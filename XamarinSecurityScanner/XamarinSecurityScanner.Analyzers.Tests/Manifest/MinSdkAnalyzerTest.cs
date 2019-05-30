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
using XamarinSecurityScanner.Analyzers.Manifest;
using XamarinSecurityScanner.Core.Manifest;

namespace XamarinSecurityScanner.Analyzers.Tests.Manifest
{
    [TestClass]
    public class MinSdkAnalyzerTest
    {
        private AndroidManifestAnalyzer _analyzer;
        private List<Vulnerability> _vulnerabilities;

        [TestInitialize]
        public void Initialize()
        {
            _analyzer = new MinSdkAnalyzer();
            _vulnerabilities = new List<Vulnerability>();
            _analyzer.VulnerabilityDiscovered += OnVulnerabilityDiscovered;
        }

        [TestMethod]
        public void MinSdkUnsupported()
        {
            AndroidManifestFile androidManifestFile = GetAndroidManifestFile("MinSdkUnsupported.xml");

            _analyzer.Analyze(androidManifestFile);
            
            Assert.AreEqual(1, _vulnerabilities.Count);
            Vulnerability vulnerability = _vulnerabilities[0];
            Assert.AreEqual("MinSdk", vulnerability.Code);
            Assert.AreEqual("App supports outdated Android version", vulnerability.Title);
            Assert.AreEqual("Apps should no longer support Android Gingerbread or lower. This version is used by less than 0.3% of all devices and the latest release was in 2011.", vulnerability.Description);
            string expectedPath = Path.Combine("TestFiles", "MinSdk", "MinSdkUnsupported.xml");
            Assert.AreEqual(expectedPath, vulnerability.FilePath);
            Assert.AreEqual("AndroidManifest.xml", vulnerability.FullyQualifiedName);
            Assert.AreEqual(9, vulnerability.LineNumber);
        }

        [TestMethod]
        public void MinSdkSupported()
        {
            AndroidManifestFile androidManifestFile = GetAndroidManifestFile("MinSdkSupported.xml");

            _analyzer.Analyze(androidManifestFile);

            Assert.AreEqual(0, _vulnerabilities.Count);
        }
        
        [TestMethod]
        public void MinSdkMissing()
        {
            AndroidManifestFile androidManifestFile = GetAndroidManifestFile("MinSdkMissing.xml");

            _analyzer.Analyze(androidManifestFile);

            Assert.AreEqual(0, _vulnerabilities.Count);
        }
        
        [TestMethod]
        public void UsesSdkMissing()
        {
            AndroidManifestFile androidManifestFile = GetAndroidManifestFile("UsesSdkMissing.xml");

            _analyzer.Analyze(androidManifestFile);

            Assert.AreEqual(0, _vulnerabilities.Count);
        }
        
        [TestMethod]
        public void MinSdkText()
        {
            AndroidManifestFile androidManifestFile = GetAndroidManifestFile("MinSdkText.xml");

            _analyzer.Analyze(androidManifestFile);

            Assert.AreEqual(0, _vulnerabilities.Count);
        }

        private void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
            _vulnerabilities.Add(vulnerability);
        }

        private static AndroidManifestFile GetAndroidManifestFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "MinSdk", fileName);
            return new AndroidManifestFile(path);
        }
    }
}
