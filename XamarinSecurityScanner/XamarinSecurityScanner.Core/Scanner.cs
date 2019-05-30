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

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Text;

[assembly: InternalsVisibleTo("XamarinSecurityScanner.Core.Tests")]
[assembly: InternalsVisibleTo("XamarinSecurityScanner.App.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] 
namespace XamarinSecurityScanner.Core
{    
    internal class Scanner : IScanner
    {
        public IFileFinder FileFinder { private get; set; }
        public List<ICsAnalyzer> CsAnalyzers { private get; set; }
        public List<IAndroidManifestAnalyzer> AndroidManifestAnalyzers { private get; set; }
        public List<ITextAnalyzer> TextAnalyzers { private get; set; }

        private string Path { get; set; }

        public Task Start(string path)
        {
            Path = path ?? throw new XamarinSecurityScannerException("Scan path must be defined.");
            
            Task csTask = StartCsAnalyzer();
            Task manifestTask = StartManifestAnalyzer();
            Task textTask = StartTextAnalyzer();

            return Task.WhenAll(csTask, manifestTask, textTask);
        }

        private Task StartCsAnalyzer()
        {
            var tasks = new List<Task>();

            ImmutableList<CsFile> csFiles = FileFinder.GetCsFiles(Path);
            CsAnalyzers.ForEach(analyzer =>
            {
                XamarinSecurityScannerLogger.Log("Started {0}.", analyzer.GetType().Name);
                Task task = Task.Run(() => csFiles.ForEach(analyzer.Analyze));
                tasks.Add(task);
            });

            return Task.WhenAll(tasks);
        }

        private Task StartManifestAnalyzer()
        {
            var tasks = new List<Task>();

            ImmutableList<AndroidManifestFile> androidManifestFiles = FileFinder.GetAndroidManifestFiles(Path);
            AndroidManifestAnalyzers.ForEach(analyzer =>
            {
                XamarinSecurityScannerLogger.Log("Started {0}.", analyzer.GetType().Name);
                Task task = Task.Run(() => androidManifestFiles.ForEach(analyzer.Analyze));
                tasks.Add(task);
            });

            return Task.WhenAll(tasks);
        }

        private Task StartTextAnalyzer()
        {
            var tasks = new List<Task>();

            ImmutableList<TextFile> textFiles = FileFinder.GetTextFiles(Path);
            TextAnalyzers.ForEach(analyzer =>
            {
                XamarinSecurityScannerLogger.Log("Started {0}.", analyzer.GetType().Name);
                Task task = Task.Run(() => textFiles.ForEach(analyzer.Analyze));
                tasks.Add(task);
            });

            return Task.WhenAll(tasks);
        }
    }
}
