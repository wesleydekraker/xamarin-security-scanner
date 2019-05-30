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

using XamarinSecurityScanner.Core;
using XamarinSecurityScanner.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System;

namespace XamarinSecurityScanner.App.Tests
{
    [TestClass]
    public class ProgramTest
    {
        private Mock<IScannerFactory> _scannerFactory;
        private Mock<IScanner> _scanner;
        private Mock<IEnvironmentWrapper> _environmentWrapper;

        [TestInitialize]
        public void Initialize()
        {
            _scanner = new Mock<IScanner>(MockBehavior.Strict);
            _scannerFactory = new Mock<IScannerFactory>(MockBehavior.Strict);
            _environmentWrapper = new Mock<IEnvironmentWrapper>(MockBehavior.Strict);
        }

        [TestMethod]
        public void NoVulnerabilities()
        {
            _scannerFactory.Setup(sf => sf.Create()).Returns(_scanner.Object);
            _scanner.Setup(sf => sf.Start("/example/path")).Returns(Task.CompletedTask);
            _environmentWrapper.Setup(ew => ew.Exit(0));

            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path"
            };
            program.OnExecute();

            _scannerFactory.Verify(sf => sf.Create(), Times.Once);
            _scanner.Verify(sf => sf.Start("/example/path"), Times.Once);
            _environmentWrapper.Verify(ew => ew.Exit(0), Times.Once);
        }

        [TestMethod]
        public void SingleVulnerability()
        {
            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path"
            };

            _scannerFactory.Setup(sf => sf.Create()).Returns(_scanner.Object);
            _scanner.Setup(sf => sf.Start("/example/path")).Returns(Task.CompletedTask).Callback(() =>
                program.OnVulnerabilityDiscovered(GetExampleVulnerability())
            );
            _environmentWrapper.Setup(ew => ew.Exit(1));

            program.OnExecute();

            _scannerFactory.Verify(sf => sf.Create(), Times.Once);
            _scanner.Verify(sf => sf.Start("/example/path"), Times.Once);
            _environmentWrapper.Verify(ew => ew.Exit(1), Times.Once);
        }

        [TestMethod]
        public void LoggingEnabled()
        {
            _scannerFactory.Setup(sf => sf.Create()).Returns(_scanner.Object);
            _scanner.Setup(sf => sf.Start("/example/path")).Returns(Task.CompletedTask);
            _environmentWrapper.Setup(ew => ew.Exit(0));

            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path",
                EnableLogging = true
            };
            program.OnExecute();
        }

        [TestMethod]
        public void IgnoreFile()
        {
            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path",
                IgnoreFile = "ignore-file.json"
            };

            _scannerFactory.Setup(sf => sf.Create()).Returns(_scanner.Object);
            _scanner.Setup(sf => sf.Start("/example/path")).Returns(Task.CompletedTask).Callback(() =>
                program.OnVulnerabilityDiscovered(GetExampleVulnerability())
            );
            _environmentWrapper.Setup(ew => ew.Exit(0));

            program.OnExecute();

            _scannerFactory.Verify(sf => sf.Create(), Times.Once);
            _scanner.Verify(sf => sf.Start("/example/path"), Times.Once);
            _environmentWrapper.Verify(ew => ew.Exit(0), Times.Once);
        }

        [TestMethod]
        public void NonExistingIgnoreFile()
        {
            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path",
                IgnoreFile = "non-existing-ignore-file.json"
            };

            Action action = () => program.OnExecute();

            var exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual(exception.Message, "Could not read ignore file: non-existing-ignore-file.json.");
        }

        [TestMethod]
        public void InvalidIgnoreFile()
        {
            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path",
                IgnoreFile = "invalid-ignore-file.json"
            };

            Action action = () => program.OnExecute();

            var exception = Assert.ThrowsException<XamarinSecurityScannerException>(action);
            Assert.AreEqual(exception.Message, "Could not parse ignore file: invalid-ignore-file.json.");
        }

        [TestMethod]
        public void CustomThreshold()
        {
            var program = new Program
            {
                ScannerFactory = _scannerFactory.Object,
                EnvironmentWrapper = _environmentWrapper.Object,
                Path = "/example/path",
                Threshold = 2
            };

            _scannerFactory.Setup(sf => sf.Create()).Returns(_scanner.Object);
            _scanner.Setup(sf => sf.Start("/example/path")).Returns(Task.CompletedTask).Callback(() =>
                program.OnVulnerabilityDiscovered(GetExampleVulnerability())
            );
            _environmentWrapper.Setup(ew => ew.Exit(0));

            program.OnExecute();

            _scannerFactory.Verify(sf => sf.Create(), Times.Once);
            _scanner.Verify(sf => sf.Start("/example/path"), Times.Once);
            _environmentWrapper.Verify(ew => ew.Exit(0));
        }

        private Vulnerability GetExampleVulnerability()
        {
            return new Vulnerability
            {
                Code = "Example",
                FullyQualifiedName = "Example.A.B.C"
            };
        }
    }
}
