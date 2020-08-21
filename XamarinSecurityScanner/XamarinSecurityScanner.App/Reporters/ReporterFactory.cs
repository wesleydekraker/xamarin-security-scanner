/*
Copyright 2020 Wesley de Kraker

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

using XamarinSecurityScanner.Core;

namespace XamarinSecurityScanner.App.Reporters
{
    class ReporterFactory : IReporterFactory
    {
        private IConsoleWrapper _consoleWrapper;

        public ReporterFactory(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public IVulnerabilityReporter Create(string outputFormat)
        {
            if (outputFormat == null)
            {
                outputFormat = "text";
            }

            var outputFormatLower = outputFormat.Trim().ToLower();

            if (outputFormatLower == "text")
            {
                return new TextReporter(_consoleWrapper);
            }
            else if (outputFormatLower == "json")
            {
                return new JsonReporter(_consoleWrapper);
            }
            else if (outputFormatLower == "csv")
            {
                return new CsvReporter(_consoleWrapper);
            }
            else if (outputFormatLower == "html")
            {
                return new HtmlReporter(_consoleWrapper);
            }

            throw new XamarinSecurityScannerException($"Unknown output format: {outputFormat}.");
        }
    }
}
