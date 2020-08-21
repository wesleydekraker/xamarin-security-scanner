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
            _consoleWrapper.WriteLine(@"<!DOCTYPE html>
<html lang=""en"">
<head>
<title>Xamarin Security Scanner</title>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
</head>
<body>
<table>
<tr>
    <th>Code</th>
    <th>Title</th>
    <th>SeverityLevel</th>
    <th>Description</th>
    <th>FilePath</th>
    <th>FullyQualifiedName</th>
    <th>LineNumber</th>
</tr>");
        }

        public override void Process(Vulnerability vulnerability)
        {
            _consoleWrapper.WriteLine($@"<tr>
    <td>{vulnerability.Code}</td>
    <td>{vulnerability.Title}</td>
    <td>{vulnerability.SeverityLevel}</td>
    <td>{vulnerability.Description}</td>
    <td>{vulnerability.FilePath}</td>
    <td>{vulnerability.FullyQualifiedName}</td>
    <td>{vulnerability.LineNumber}</td>
</tr>");
        }

        public override void Finish()
        {
            _consoleWrapper.WriteLine(@"</table>
</body>
</html>");
        }
    }
}
