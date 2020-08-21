using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text;
using XamarinSecurityScanner.App.Reporters;
using XamarinSecurityScanner.Core.Models;

namespace XamarinSecurityScanner.App.Tests.Reporters
{
    [TestClass]
    public class CsvReporterTest
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
            var reporter = new CsvReporter(_consoleWrapper.Object);

            reporter.Start();
            reporter.Finish();

            Assert.AreEqual(@"Code;Title;SeverityLevel;Description;FilePath;FullyQualifiedName;LineNumber
", _output.ToString());
        }

        [TestMethod]
        public void OneVulnerability()
        {
            var reporter = new CsvReporter(_consoleWrapper.Object);

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

            Assert.AreEqual(@"Code;Title;SeverityLevel;Description;FilePath;FullyQualifiedName;LineNumber
ExampleCode;Example Vulnerability;Critical;Description here.;C:\Program.cs;Namespace.Class;10
", _output.ToString());
        }

        [TestMethod]
        public void TwoVulnerabilities()
        {
            var reporter = new CsvReporter(_consoleWrapper.Object);

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

            Assert.AreEqual(@"Code;Title;SeverityLevel;Description;FilePath;FullyQualifiedName;LineNumber
ExampleCode;Example Vulnerability;Critical;Description here.;C:\Program.cs;Namespace.Class;10
ExampleCode2;Example Vulnerability;Critical;Description here.;C:\Program.cs;Namespace.Class;20
", _output.ToString());
        }
    }
}
