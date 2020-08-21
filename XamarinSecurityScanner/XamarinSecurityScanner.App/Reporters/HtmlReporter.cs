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
    class HtmlReporter : BaseReporter
    {
        private readonly IConsoleWrapper _consoleWrapper;

        public HtmlReporter(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public override void Start()
        {
            _consoleWrapper.WriteLine("<!DOCTYPE html>");
            _consoleWrapper.WriteLine("<html lang=\"en\">");
            _consoleWrapper.WriteLine("<head>");
            _consoleWrapper.WriteLine("<title>Xamarin Security Scanner</title>");
            _consoleWrapper.WriteLine("<meta charset=\"utf-8\">");
            _consoleWrapper.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            _consoleWrapper.WriteLine("</head>");
            _consoleWrapper.WriteLine("<body>");
            _consoleWrapper.WriteLine("<table>");

            AddHeading("Code", "Title", "SeverityLevel", "Description", "FilePath", "FullyQualifiedName", "LineNumber");
        }

        public override void Process(Vulnerability vulnerability)
        {
            AddRow(vulnerability.Code, vulnerability.Title, vulnerability.SeverityLevel.ToString(), vulnerability.Description,
                   vulnerability.FilePath, vulnerability.FullyQualifiedName, vulnerability.LineNumber.ToString());
        }

        public override void Finish()
        {
            _consoleWrapper.WriteLine("</table>");
            _consoleWrapper.WriteLine("</body>");
            _consoleWrapper.WriteLine("</html>");
        }

        private void AddHeading(params string[] headers)
        {
            _consoleWrapper.WriteLine("<tr>");

            foreach (var header in headers)
            {
                _consoleWrapper.WriteLine($"    <th>{header}</th>");
            }

            _consoleWrapper.WriteLine("</tr>");
        }

        private void AddRow(params string[] values)
        {
            _consoleWrapper.WriteLine("<tr>");

            foreach (var value in values)
            {
                _consoleWrapper.WriteLine($"    <td>{value}</td>");
            }

            _consoleWrapper.WriteLine("</tr>");
        }
    }
}
