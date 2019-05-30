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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamarinSecurityScanner.Core.Manifest;

namespace XamarinSecurityScanner.Core.Tests.Manifest
{
    [TestClass]
    public class AndroidManifestFileTest
    {
        [TestMethod]
        public void InvalidDocument()
        {
            AndroidManifestFile manifestFile = GetAndroidManifestFile("InvalidDocument");

            XElement element = manifestFile.GetXElement();

            Assert.AreEqual("Content", element.Value);
        }

        [TestMethod]
        public void NonExistingFile()
        {
            AndroidManifestFile manifestFile = GetAndroidManifestFile("NonExistingFile.xml");

            XElement element = manifestFile.GetXElement();

            Assert.AreEqual("Content", element.Value);
        }

        private static AndroidManifestFile GetAndroidManifestFile(string fileName)
        {
            string path = Path.Combine("TestFiles", "Manifest", fileName);
            return new AndroidManifestFile(path);
        }
    }
}
