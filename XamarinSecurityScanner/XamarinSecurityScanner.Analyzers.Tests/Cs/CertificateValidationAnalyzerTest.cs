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
using XamarinSecurityScanner.Core.Cs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XamarinSecurityScanner.Core.Models;
using XamarinSecurityScanner.Analyzers.Cs;

namespace XamarinSecurityScanner.Analyzers.Tests.Cs
{
    [TestClass]
    public class CertificateValidationAnalyzerTest
    {
        private CsAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new CertificateValidationAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }

        [TestMethod]
        public void SslCertificate()
        {
            CsFile csFile = GetCsFile("CertificateValidation.cs.test");

            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("CertificateValidation", vulnerability.Code);
            Assert.AreEqual("Certificate validation overwritten", vulnerability.Title);
            Assert.AreEqual("Certificate validation callback is overwritten. This may open the door to man-in-the-middle attacks.", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "CertificateValidation", "CertificateValidation.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.CertificateValidation", vulnerability.FullyQualifiedName);
            Assert.AreEqual(17, vulnerability.LineNumber);
        }

        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static CsFile GetCsFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "CertificateValidation", fileName);
            return new CsFile(path);
        }
    }
}
