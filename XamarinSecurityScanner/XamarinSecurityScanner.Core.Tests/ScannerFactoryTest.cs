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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamarinSecurityScanner.Core.Cs;
using System.Collections.Generic;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Text;

namespace XamarinSecurityScanner.Core.Tests
{
    [TestClass]
    public class ScannerFactoryTest
    {
        [TestMethod]
        public void Create()
        {
            var scannerFactory = new ScannerFactory
            {
                CsAnalyzers = new List<ICsAnalyzer>(),
                AndroidManifestAnalyzers = new List<IAndroidManifestAnalyzer>(),
                TextAnalyzers = new List<ITextAnalyzer>()
            };

            IScanner scanner = scannerFactory.Create();

            Assert.IsNotNull(scanner);
        }

        [TestMethod]
        public void CsAnalyzersIsNull()
        {
            var scannerFactory = new ScannerFactory
            {
                AndroidManifestAnalyzers = new List<IAndroidManifestAnalyzer>(),
                TextAnalyzers = new List<ITextAnalyzer>()
            };

            Action action = () => scannerFactory.Create();

            var exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual("The property \"CsAnalyzers\" cannot be null in ScannerFactory.", exception.Message);
        }

        [TestMethod]
        public void AndroidManifestAnalyzersIsNull()
        {
            var scannerFactory = new ScannerFactory
            {
                CsAnalyzers = new List<ICsAnalyzer>(),
                TextAnalyzers = new List<ITextAnalyzer>()
            };

            Action action = () => scannerFactory.Create();

            var exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual("The property \"AndroidManifestAnalyzers\" cannot be null in ScannerFactory.", exception.Message);
        }

        [TestMethod]
        public void TextAnalyzersIsNull()
        {
            var scannerFactory = new ScannerFactory
            {
                CsAnalyzers = new List<ICsAnalyzer>(),
                AndroidManifestAnalyzers = new List<IAndroidManifestAnalyzer>()
            };

            Action action = () => scannerFactory.Create();

            var exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual("The property \"TextAnalyzers\" cannot be null in ScannerFactory.", exception.Message);
        }
    }
}
