using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text;
using XamarinSecurityScanner.App.Reporters;
using XamarinSecurityScanner.Core.Models;

namespace XamarinSecurityScanner.App.Tests.Reporters
{
    [TestClass]
    public class JsonReporterTest
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
                .Callback<string>(v => _output.Append(v + Environment.NewLine));
            _consoleWrapper.Setup(c => c.WriteLine(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((v, a) => _output.Append(string.Format(v, a) + Environment.NewLine));
        }

        [TestMethod]
        public void NoVulnerabilities()
        {
            var reporter = new JsonReporter(_consoleWrapper.Object);

            reporter.Start();
            reporter.Finish();

            Assert.AreEqual(@"[]
", _output.ToString().Replace(Environment.NewLine, CurrentDocumentNewLine));
        }

        [TestMethod]
        public void OneVulnerability()
        {
            var reporter = new JsonReporter(_consoleWrapper.Object);

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

            Assert.AreEqual(@"[
  {
    ""Code"": ""ExampleCode"",
    ""Title"": ""Example Vulnerability"",
    ""SeverityLevel"": ""Critical"",
    ""Description"": ""Description here."",
    ""FilePath"": ""C:\\Program.cs"",
    ""FullyQualifiedName"": ""Namespace.Class"",
    ""LineNumber"": 10
  }
]
", _output.ToString().Replace(Environment.NewLine, CurrentDocumentNewLine));
        }

        [TestMethod]
        public void TwoVulnerabilities()
        {
            var reporter = new JsonReporter(_consoleWrapper.Object);

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

            Assert.AreEqual(@"[
  {
    ""Code"": ""ExampleCode"",
    ""Title"": ""Example Vulnerability"",
    ""SeverityLevel"": ""Critical"",
    ""Description"": ""Description here."",
    ""FilePath"": ""C:\\Program.cs"",
    ""FullyQualifiedName"": ""Namespace.Class"",
    ""LineNumber"": 10
  },
  {
    ""Code"": ""ExampleCode2"",
    ""Title"": ""Example Vulnerability"",
    ""SeverityLevel"": ""Critical"",
    ""Description"": ""Description here."",
    ""FilePath"": ""C:\\Program.cs"",
    ""FullyQualifiedName"": ""Namespace.Class"",
    ""LineNumber"": 20
  }
]
", _output.ToString().Replace(Environment.NewLine, CurrentDocumentNewLine));
        }
    }
}
