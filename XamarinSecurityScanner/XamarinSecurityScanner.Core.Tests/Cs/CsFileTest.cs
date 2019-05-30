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

using System.IO;
using System.Linq;
using XamarinSecurityScanner.Core.Cs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XamarinSecurityScanner.Core.Tests.Cs
{
    [TestClass]
    public class CsFileTest
    {
        [TestMethod]
        public void NonExistingFile()
        {
            CsFile csFile = GetCsFile("NonExistingFile.cs");

            CompilationUnitSyntax unit = csFile.GetUnit();
            
            Assert.AreEqual("", unit.ToFullString());
            Assert.AreEqual(0, unit.DescendantNodes().Count());
        }

        private static CsFile GetCsFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "NonExistingFolder", fileName);
            return new CsFile(path);
        }
    }
}
