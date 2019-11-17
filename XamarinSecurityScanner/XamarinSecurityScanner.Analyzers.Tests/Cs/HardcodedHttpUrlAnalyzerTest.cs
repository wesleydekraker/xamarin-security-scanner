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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XamarinSecurityScanner.Core.Models;
using XamarinSecurityScanner.Analyzers.Cs;
using XamarinSecurityScanner.Core.Cs;

namespace XamarinSecurityScanner.Analyzers.Tests.Cs
{
    [TestClass]
    public class HardcodedHttpUrlAnalyzerTest
    {
        private CsAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new HardcodedHttpUrlAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }
        
        [TestMethod]
        public void HardcodedHttpUrl()
        {
            CsFile csFile = GetCsFile("HardcodedHttpUrl.cs.test");

            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("HardcodedHttpUrl", vulnerability.Code);
            Assert.AreEqual("Hardcoded HTTP URL found", vulnerability.Title);
            Assert.AreEqual(SeverityLevel.Low, vulnerability.SeverityLevel);
            Assert.AreEqual("HTTP traffic may not be encrypted, which opens the door to man-in-the-middle attacks. HTTP URL: http://www.example.com.", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "HardcodedHttpUrl", "HardcodedHttpUrl.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.HardcodedHttpUrl", vulnerability.FullyQualifiedName);
            Assert.AreEqual(16, vulnerability.LineNumber);
        }

        [TestMethod]
        public void HardcodedHttpUrlAsConst()
        {
            CsFile csFile = GetCsFile("HardcodedHttpUrlVariants.cs.test");

            _analyzer.Analyze(csFile);

            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("HardcodedHttpUrl", vulnerability.Code);
            Assert.AreEqual("Hardcoded HTTP URL found", vulnerability.Title);
            Assert.AreEqual(SeverityLevel.Low, vulnerability.SeverityLevel);
            Assert.AreEqual("HTTP traffic may not be encrypted, which opens the door to man-in-the-middle attacks. HTTP URL: http://www.example.com.", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "HardcodedHttpUrl", "HardcodedHttpUrlVariants.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.HardcodedHttpUrlVariants", vulnerability.FullyQualifiedName);
            Assert.AreEqual(10, vulnerability.LineNumber);
        }

        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static CsFile GetCsFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "HardcodedHttpUrl", fileName);
            return new CsFile(path);
        }
    }
}
