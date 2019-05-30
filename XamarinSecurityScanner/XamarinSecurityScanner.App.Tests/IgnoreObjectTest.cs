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

using XamarinSecurityScanner.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace XamarinSecurityScanner.App.Tests
{
    [TestClass]
    public class IgnoreObjectTest
    {
        private IgnoreObject _ignoreObject;

        [TestInitialize]
        public void Initialize()
        {
            _ignoreObject = new IgnoreObject
            {
                IgnoredVulnerabilities = new List<IgnoredVulnerability>
                {
                    new IgnoredVulnerability
                    {
                        FullyQualifiedName = "BankingApp.TestFiles",
                        VulnerabilityCode = "Example"
                    }
                }
            };
        }

        [TestMethod]
        public void MatchingFullyQualifiedName()
        {
            var vulnerability = new Vulnerability
            {
                FullyQualifiedName = "BankingApp.TestFiles",
                Code = "AnotherExample"
            };

            bool result = _ignoreObject.IsIgnored(vulnerability);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchingCode()
        {
            var vulnerability = new Vulnerability
            {
                FullyQualifiedName = "AnotherBankingApp.TestFiles",
                Code = "Example"
            };

            bool result = _ignoreObject.IsIgnored(vulnerability);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CompleteMatch()
        {
            var vulnerability = new Vulnerability
            {
                FullyQualifiedName = "BankingApp.TestFiles",
                Code = "Example"
            };

            bool result = _ignoreObject.IsIgnored(vulnerability);
            Assert.IsTrue(result);
        }
    }
}
