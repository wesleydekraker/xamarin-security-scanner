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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace XamarinSecurityScanner.Core.Tests
{
    [TestClass]
    public class ScannerTest
    {
        private Mock<IFileFinder> _fileFinder;
        private Mock<ICsAnalyzer> _csAnalyzer;
        private Mock<IAndroidManifestAnalyzer> _androidManifestAnalyzer;
        private Mock<ITextAnalyzer> _textAnalyzer;
        private Scanner _scanner;

        [TestInitialize]
        public void Initialize()
        {
            _fileFinder = new Mock<IFileFinder>(MockBehavior.Strict);
            _csAnalyzer = new Mock<ICsAnalyzer>(MockBehavior.Strict);
            _androidManifestAnalyzer = new Mock<IAndroidManifestAnalyzer>(MockBehavior.Strict);
            _textAnalyzer = new Mock<ITextAnalyzer>(MockBehavior.Strict);

            _scanner = new Scanner
            {
                FileFinder = _fileFinder.Object,
                CsAnalyzers = new List<ICsAnalyzer>
                {
                    _csAnalyzer.Object
                },
                AndroidManifestAnalyzers = new List<IAndroidManifestAnalyzer>
                {
                    _androidManifestAnalyzer.Object
                },
                TextAnalyzers = new List<ITextAnalyzer>
                {
                    _textAnalyzer.Object
                }
            };
        }
        
        [TestMethod]
        public async Task StartScan()
        {
            _fileFinder.Setup(f => f.GetCsFiles("/example/path"))
                .Returns(ImmutableList.Create(new CsFile("File.cs")));

            _fileFinder.Setup(f => f.GetAndroidManifestFiles("/example/path"))
                .Returns(ImmutableList.Create(new AndroidManifestFile("File.cs")));

            _fileFinder.Setup(f => f.GetTextFiles("/example/path"))
                .Returns(ImmutableList.Create(new TextFile("File.cs")));

            _csAnalyzer.Setup(a => a.Analyze(It.IsAny<CsFile>()));
            _androidManifestAnalyzer.Setup(a => a.Analyze(It.IsAny<AndroidManifestFile>()));
            _textAnalyzer.Setup(a => a.Analyze(It.IsAny<TextFile>()));

            await _scanner.Start("/example/path");
            
            _fileFinder.Verify(f => f.GetCsFiles("/example/path"), Times.Once);
            _fileFinder.Verify(f => f.GetAndroidManifestFiles("/example/path"), Times.Once);
            _fileFinder.Verify(f => f.GetTextFiles("/example/path"), Times.Once);
            _csAnalyzer.Verify(a => a.Analyze(It.Is<CsFile>(f => f.FilePath == "File.cs")), Times.Once);
            _androidManifestAnalyzer.Verify(a => a.Analyze(It.Is<AndroidManifestFile>(f => f.FilePath == "File.cs")), Times.Once);
            _textAnalyzer.Verify(a => a.Analyze(It.Is<TextFile>(f => f.FilePath == "File.cs")), Times.Once);
        }
        
                
        [TestMethod]
        public async Task PathIsNull()
        {
            Func<Task> action = () => _scanner.Start(null);

            XamarinSecurityScannerException exception = await Assert.ThrowsExceptionAsync<XamarinSecurityScannerException>(action);
            Assert.AreEqual("Scan path must be defined.", exception.Message);
        }
    }
}
