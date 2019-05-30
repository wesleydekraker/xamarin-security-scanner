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
using System.Reflection;
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XamarinSecurityScanner.Analyzers.Tests
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        public void Create()
        {
            var analyzerFactory = new AnalyzerFactory
            {
                Assembly = Assembly.GetExecutingAssembly(),
                VulnerabilityDiscovered = OnVulnerabilityDiscovered
            };

            List<ICsAnalyzer> csAnalyzers = analyzerFactory.GetCsAnalyzers();

            Assert.AreEqual(1, csAnalyzers.Count);
            ICsAnalyzer analyzer = csAnalyzers[0];
            Assert.IsInstanceOfType(analyzer, typeof(ExampleAnalyzer));
        }
        private static void OnVulnerabilityDiscovered(Vulnerability vulnerability)
        {
        }
    }
}
