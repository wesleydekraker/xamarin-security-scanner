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
    public class PhoneNumberAccessAnalyzerTest
    {
        private CsAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new PhoneNumberAccessAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }

        [TestMethod]
        public void PhoneNumberAccess()
        {
            CsFile csFile = GetCsFile("PhoneNumberAccess.cs.test");

            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("PhoneNumberAccess", vulnerability.Code);
            Assert.AreEqual("Access to phone number", vulnerability.Title);
            Assert.AreEqual(SeverityLevel.Low, vulnerability.SeverityLevel);
            Assert.AreEqual("Be careful accessing the phone number of your user. This is personally identifying information (PII).", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "PhoneNumberAccess", "PhoneNumberAccess.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.UniqueIdentifiers", vulnerability.FullyQualifiedName);
            Assert.AreEqual(18, vulnerability.LineNumber);
        }

        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static CsFile GetCsFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "PhoneNumberAccess", fileName);
            return new CsFile(path);
        }
    }
}
