using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text;
using XamarinSecurityScanner.App.Reporters;
using XamarinSecurityScanner.Core.Models;

namespace XamarinSecurityScanner.App.Tests.Reporters
{
    [TestClass]
    public class HtmlReporterTest
    {
        private const string CurrentDocumentNewLine = "\r\n";
        private Mock<IConsoleWrapper> _consoleWrapper;
        private StringBuilder _output;

        [TestInitialize]
        public void Initialize()
        {
            _output = new StringBuilder();

            _consoleWrapper = new Mock<IConsoleWrapper>(MockBehavior.Strict);
            _consoleWrapper.Setup(c => c.WriteLine(It.IsAny<string>()))
                .Callback<string>(v => _output.Append(v + CurrentDocumentNewLine));
            _consoleWrapper.Setup(c => c.WriteLine(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((v, a) => _output.Append(string.Format(v, a) + CurrentDocumentNewLine));
        }

        [TestMethod]
        public void NoVulnerabilities()
        {
            var reporter = new HtmlReporter(_consoleWrapper.Object);

            reporter.Start();
            reporter.Finish();

            Assert.AreEqual(@"<!DOCTYPE html>
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
</tr>
</table>
</body>
</html>
", _output.ToString());
        }

        [TestMethod]
        public void OneVulnerability()
        {
            var reporter = new HtmlReporter(_consoleWrapper.Object);

            reporter.Start();
            reporter.Report(new Vulnerability
            {
                Code = "ExampleCode",
                Title = "Example Vulnerability",
                SeverityLevel = SeverityLevel.Critical,
                Description = "Description here.",
                FilePath = "C:\\Program.cs",
                FullyQualifiedName = "Namespace.Class",
                LineNumber = 10
            });
            reporter.Finish();

            Assert.AreEqual(@"<!DOCTYPE html>
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
</tr>
<tr>
    <td>ExampleCode</td>
    <td>Example Vulnerability</td>
    <td>Critical</td>
    <td>Description here.</td>
    <td>C:\Program.cs</td>
    <td>Namespace.Class</td>
    <td>10</td>
</tr>
</table>
</body>
</html>
", _output.ToString());
        }

        [TestMethod]
        public void TwoVulnerabilities()
        {
            var reporter = new HtmlReporter(_consoleWrapper.Object);

            reporter.Start();
            reporter.Report(new Vulnerability
            {
                Code = "ExampleCode",
                Title = "Example Vulnerability",
                SeverityLevel = SeverityLevel.Critical,
                Description = "Description here.",
                FilePath = "C:\\Program.cs",
                FullyQualifiedName = "Namespace.Class",
                LineNumber = 10
            });
            reporter.Report(new Vulnerability
            {
                Code = "ExampleCode2",
                Title = "Example Vulnerability",
                SeverityLevel = SeverityLevel.Critical,
                Description = "Description here.",
                FilePath = "C:\\Program.cs",
                FullyQualifiedName = "Namespace.Class",
                LineNumber = 20
            });
            reporter.Finish();

            Assert.AreEqual(@"<!DOCTYPE html>
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
</tr>
<tr>
    <td>ExampleCode</td>
    <td>Example Vulnerability</td>
    <td>Critical</td>
    <td>Description here.</td>
    <td>C:\Program.cs</td>
    <td>Namespace.Class</td>
    <td>10</td>
</tr>
<tr>
    <td>ExampleCode2</td>
    <td>Example Vulnerability</td>
    <td>Critical</td>
    <td>Description here.</td>
    <td>C:\Program.cs</td>
    <td>Namespace.Class</td>
    <td>20</td>
</tr>
</table>
</body>
</html>
", _output.ToString());
        }
    }
}
