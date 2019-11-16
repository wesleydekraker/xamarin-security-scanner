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
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Text;

namespace XamarinSecurityScanner.Core.Tests
{
    [TestClass]
    public class FileFinderTest
    {
        private FileFinder _fileFinder;
        private string _fileFinderPath = Path.Combine("TestFiles", "FileFinder");

        [TestInitialize]
        public void Initialize()
        {
            _fileFinder = new FileFinder();
        }

        [TestMethod]
        public void GetCsFiles()
        {
            _fileFinder.DirectoryWrapper = new DirectoryWrapper();
            ImmutableList<CsFile> csFiles = _fileFinder.GetCsFiles(_fileFinderPath);

            Assert.AreEqual(0, csFiles.Count);
        }

        [TestMethod]
        public void GetAndroidManifestFiles()
        {
            _fileFinder.DirectoryWrapper = new DirectoryWrapper();
            ImmutableList<AndroidManifestFile> manifestFiles = _fileFinder.GetAndroidManifestFiles(_fileFinderPath);

            Assert.AreEqual(1, manifestFiles.Count);
            string filePath = Path.Combine("TestFiles", "FileFinder", "AndroidManifest.xml");
            Assert.AreEqual(manifestFiles[0].FilePath, filePath);
        }

        [TestMethod]
        public void GetTextFiles()
        {
            _fileFinder.DirectoryWrapper = new DirectoryWrapper();
            ImmutableList<TextFile> manifestFiles = _fileFinder.GetTextFiles(_fileFinderPath);

            Assert.AreEqual(3, manifestFiles.Count);
            string manifestFilePath = Path.Combine("TestFiles", "FileFinder", "AndroidManifest.xml");
            Assert.IsTrue(manifestFiles.Any(file => file.FilePath == manifestFilePath));
            string examplePath = Path.Combine("TestFiles", "FileFinder", "Example.txt");
            Assert.IsTrue(manifestFiles.Any(file => file.FilePath == examplePath));
            string exampleCsPath = Path.Combine("TestFiles", "FileFinder", "Example.cs.test");
            Assert.IsTrue(manifestFiles.Any(file => file.FilePath == exampleCsPath));
        }

        [TestMethod]
        public void DirectoryWrapperThrowsDirectoryNotFoundException()
        {
            var directoryWrapper = new Mock<IDirectoryWrapper>(MockBehavior.Strict);
            directoryWrapper.Setup(f => f.GetFiles("/example/path", "*.cs"))
                .Throws<DirectoryNotFoundException>();
            _fileFinder.DirectoryWrapper = directoryWrapper.Object;

            Action action = () => _fileFinder.GetFiles("/example/path", "*.cs");

            XamarinSecurityScannerException exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual("Incorrect scan path. Directory not found.", exception.Message);
        }
        
        [TestMethod]
        public void DirectoryWrapperThrowsPathTooLongException()
        {
            var directoryWrapper = new Mock<IDirectoryWrapper>(MockBehavior.Strict);
            directoryWrapper.Setup(f => f.GetFiles("/example/path", "*.cs"))
                .Throws<PathTooLongException>();
            _fileFinder.DirectoryWrapper = directoryWrapper.Object;

            Action action = () => _fileFinder.GetFiles("/example/path", "*.cs");

            XamarinSecurityScannerException exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual("Scan path is too long.", exception.Message);
        }
        
        [TestMethod]
        public void DirectoryWrapperThrowsIoException()
        {
            var directoryWrapper = new Mock<IDirectoryWrapper>(MockBehavior.Strict);
            directoryWrapper.Setup(f => f.GetFiles("/example/path", "*.cs"))
                .Throws<IOException>();
            _fileFinder.DirectoryWrapper = directoryWrapper.Object;

            Action action = () => _fileFinder.GetFiles("/example/path", "*.cs");

            XamarinSecurityScannerException exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual("Scan path must be a folder.", exception.Message);
        }
    }
}
