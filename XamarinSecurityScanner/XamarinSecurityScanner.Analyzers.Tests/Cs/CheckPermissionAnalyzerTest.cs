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
    public class CheckPermissionAnalyzerTest
    {
        private CsAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new CheckPermissionAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }

        [TestMethod]
        public void CheckPermission()
        {
            CsFile csFile = GetCsFile("CheckPermission.cs.test");
            
            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(6, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("CheckPermission", vulnerability.Code);
            Assert.AreEqual("Permissions may not be enforced", vulnerability.Title);
            Assert.AreEqual("Permissions may not be enforced when using this method in an exported component: CheckCallingOrSelfPermission(...).", vulnerability.Description);
            var expectedPath = Path.Combine("TestFiles", "CheckPermission", "CheckPermission.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.CheckPermission", vulnerability.FullyQualifiedName);
            Assert.AreEqual(22, vulnerability.LineNumber);
        }

        [TestMethod]
        public void CheckPermissionVariants()
        {
            CsFile csFile = GetCsFile("CheckPermissionVariants.cs.test");

            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(2, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("CheckPermission", vulnerability.Code);
            Assert.AreEqual("Permissions may not be enforced", vulnerability.Title);
            Assert.AreEqual("Permissions may not be enforced when using this method in an exported component: CheckCallingOrSelfPermission(...).", vulnerability.Description);
            var expectedPath = Path.Combine("TestFiles", "CheckPermission", "CheckPermissionVariants.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.CheckPermissionVariants", vulnerability.FullyQualifiedName);
            Assert.AreEqual(18, vulnerability.LineNumber);
        }
        
        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static CsFile GetCsFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "CheckPermission", fileName);
            return new CsFile(path);
        }
    }
}
