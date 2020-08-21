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

using XamarinSecurityScanner.Core.Models;

namespace XamarinSecurityScanner.App.Reporters
{
    class CsvReporter : BaseReporter
    {
        private static readonly string _separator = ";";

        private readonly IConsoleWrapper _consoleWrapper;

        public CsvReporter(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public override void Start()
        {
            var vulnerabilityArray = new string[]
            {
                "Code",
                "Title",
                "SeverityLevel",
                "Description",
                "FilePath",
                "FullyQualifiedName",
                "LineNumber"
            };

            _consoleWrapper.WriteLine(string.Join(_separator, vulnerabilityArray));
        }

        public override void Process(Vulnerability vulnerability)
        {
            var vulnerabilityArray = new string[]
            {
                vulnerability.Code,
                vulnerability.Title,
                vulnerability.SeverityLevel.ToString(),
                vulnerability.Description,
                vulnerability.FilePath,
                vulnerability.FullyQualifiedName,
                vulnerability.LineNumber.ToString()
            };

            _consoleWrapper.WriteLine(string.Join(_separator, vulnerabilityArray));
        }

        public override void Finish()
        {
        }
    }
}
