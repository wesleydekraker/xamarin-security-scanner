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
    class TextReporter : BaseReporter
    {
        private readonly IConsoleWrapper _consoleWrapper;

        public TextReporter(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public override void Start()
        {
        }

        public override void Process(Vulnerability vulnerability)
        {
            _consoleWrapper.WriteLine($"Code: {vulnerability.Code}");
            _consoleWrapper.WriteLine($"Title: {vulnerability.Title}");
            _consoleWrapper.WriteLine($"SeverityLevel: {vulnerability.SeverityLevel}");
            _consoleWrapper.WriteLine($"Description: {vulnerability.Description}");
            _consoleWrapper.WriteLine($"File path: {vulnerability.FilePath}");
            _consoleWrapper.WriteLine($"Fully qualified name: {vulnerability.FullyQualifiedName}");
            _consoleWrapper.WriteLine($"Line number: {vulnerability.LineNumber}");
            _consoleWrapper.WriteLine("");
        }

        public override void Finish()
        {
            _consoleWrapper.WriteLine("Total vulnerabilities: {0}", VulnerabilityCount);
        }
    }
}
