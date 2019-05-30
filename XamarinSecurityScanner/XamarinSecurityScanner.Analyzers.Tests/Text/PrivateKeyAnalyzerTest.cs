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
using XamarinSecurityScanner.Core.Text;
using XamarinSecurityScanner.Analyzers.Text;

namespace XamarinSecurityScanner.Analyzers.Tests.Text
{
    [TestClass]
    public class PrivateKeyAnalyzerTest
    {
        private TextAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new PrivateKeyAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }

        [TestMethod]
        public void PrivateKey()
        {
            TextFile textFile = GetTextFile("id");

            _analyzer.Analyze(textFile);
            
            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("PrivateKey", vulnerability.Code);
            Assert.AreEqual("App contains a private key", vulnerability.Title);
            Assert.AreEqual("Private keys should never be embedded in your app.", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "PrivateKey", "id");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("id", vulnerability.FullyQualifiedName);
            Assert.AreEqual(0, vulnerability.LineNumber);
        }

        [TestMethod]
        public void RsaPrivateKey()
        {
            TextFile textFile = GetTextFile("id_rsa");

            _analyzer.Analyze(textFile);

            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("PrivateKey", vulnerability.Code);
            Assert.AreEqual("App contains a private key", vulnerability.Title);
            Assert.AreEqual("Private keys should never be embedded in your app.", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "PrivateKey", "id_rsa");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("id_rsa", vulnerability.FullyQualifiedName);
            Assert.AreEqual(0, vulnerability.LineNumber);
        }

        [TestMethod]
        public void OnlyBeginKeyword()
        {
            TextFile textFile = GetTextFile("only_begin");

            _analyzer.Analyze(textFile);

            Assert.AreEqual(0, _vulnerabilities.Count);
        }

        [TestMethod]
        public void WithoutBeginKeyword()
        {
            TextFile textFile = GetTextFile("without_begin");

            _analyzer.Analyze(textFile);

            Assert.AreEqual(0, _vulnerabilities.Count);
        }

        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static TextFile GetTextFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "PrivateKey", fileName);
            return new TextFile(path);
        }
    }
}
