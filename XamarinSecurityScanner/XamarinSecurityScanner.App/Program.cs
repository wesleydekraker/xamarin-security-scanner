/*
Copyright 2019 Info Support B.V.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using XamarinSecurityScanner.Analyzers;
using XamarinSecurityScanner.Core;
using XamarinSecurityScanner.Core.Models;
using McMaster.Extensions.CommandLineUtils;
using System.Text.Json;

[assembly: InternalsVisibleTo("XamarinSecurityScanner.App.Tests")]
namespace XamarinSecurityScanner.App
{
    internal class Program
    {
        public IScannerFactory ScannerFactory { private get; set; }

        public IEnvironmentWrapper EnvironmentWrapper { private get; set; }

        [Required]
        [Option(Description = "Path to scan")]
        public string Path { private get; set; }

        [Option(Description = "Vulnerability threshold")]
        public int Threshold { private get; set; } = 1;

        [Option(Description = "Enable logging")]
        public bool EnableLogging { private get; set; }

        [Option(Description = "Path to ignore file")]
        public string IgnoreFile { private get; set; }

        private IgnoreObject _ignoreObject;

        private int _vulnerabilityCount;

        public static int Main(string[] args) {
            try
            {
                return CommandLineApplication.Execute<Program>(args);
            }
            catch (XamarinSecurityScannerException exception)
            {
                Console.WriteLine(exception.Message);
                return 1;
            }
        }

        public Program()
        {
            AnalyzerFactory analyzerFactory = new AnalyzerFactory
            {
                VulnerabilityDiscovered = OnVulnerabilityDiscovered
            };

            ScannerFactory = new ScannerFactory
            {
                CsAnalyzers = analyzerFactory.GetCsAnalyzers(),
                AndroidManifestAnalyzers = analyzerFactory.GetAndroidManifestAnalyzers(),
                TextAnalyzers = analyzerFactory.GetTextAnalyzers()
            };

            EnvironmentWrapper = new EnvironmentWrapper();
        }

        public void OnExecute()
        {
            if (EnableLogging)
            {
                XamarinSecurityScannerLogger.LogEvent = Console.WriteLine;
            }

            if (IgnoreFile != null)
            {
                SetIgnoreObject();
            }

            IScanner scanner = ScannerFactory.Create();

            Task task = scanner.Start(Path);
            task.Wait();

            Console.WriteLine("Total vulnerabilities: {0}", _vulnerabilityCount);
            EnvironmentWrapper.Exit(_vulnerabilityCount < Threshold ? 0 : 1);
        }

        private void SetIgnoreObject()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                string ignoreText = File.ReadAllText(IgnoreFile);
                _ignoreObject = JsonSerializer.Deserialize<IgnoreObject>(ignoreText, options);
            }
            catch (IOException)
            {
                throw new XamarinSecurityScannerException($"Could not read ignore file: {IgnoreFile}.");
            }
            catch (JsonException)
            {
                throw new XamarinSecurityScannerException($"Could not parse ignore file: {IgnoreFile}.");
            }
        }
        
        public void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            if (_ignoreObject != null && _ignoreObject.IsIgnored(vulnerability))
                return;
            
            Console.WriteLine(vulnerability);
            Interlocked.Increment(ref _vulnerabilityCount);
        }
    }
}
