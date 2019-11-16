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
    public class ExternalStorageAnalyzerTest
    {
        private CsAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new ExternalStorageAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }

        [TestMethod]
        public void ExternalStorage()
        {
            CsFile csFile = GetCsFile("ExternalStorage.cs.test");

            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(4, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("ExternalStorage", vulnerability.Code);
            Assert.AreEqual("External storage is used", vulnerability.Title);
            Assert.AreEqual("Files on external storage can be accessed by any app. Check this method: GetExternalFilesDir(...).", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "ExternalStorage", "ExternalStorage.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.ExternalStorage", vulnerability.FullyQualifiedName);
            Assert.AreEqual(15, vulnerability.LineNumber);
        }

        [TestMethod]
        public void ExternalStorageVariants()
        {
            CsFile csFile = GetCsFile("ExternalStorageVariants.cs.test");

            _analyzer.Analyze(csFile);
            
            Assert.AreEqual(2, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("ExternalStorage", vulnerability.Code);
            Assert.AreEqual("External storage is used", vulnerability.Title);
            Assert.AreEqual("Files on external storage can be accessed by any app. Check this method: GetExternalFilesDir(...).", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "ExternalStorage", "ExternalStorageVariants.cs.test");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("BankingApp.TestFiles.ExternalStorageVariants", vulnerability.FullyQualifiedName);
            Assert.AreEqual(15, vulnerability.LineNumber);
        }

        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static CsFile GetCsFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "ExternalStorage", fileName);
            return new CsFile(path);
        }
    }
}
