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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace XamarinSecurityScanner.Core.Tests
{
    [TestClass]
    public class QualifiedNameResolverTest
    {
        [TestMethod]
        public void SyntaxNodeIsNull()
        {
            string qualifiedName = QualifiedNameResolver.Resolve(null);

            Assert.AreEqual("", qualifiedName);
        }

        [TestMethod]
        public void SingleNamespace()
        {
            var unit = GetUnit("SingleNamespace.cs.test");
            var methodDeclaration = unit.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            string qualifiedName = QualifiedNameResolver.Resolve(methodDeclaration);

            Assert.AreEqual("BankingApp.TestFiles.SingleNamespace", qualifiedName);
        }

        [TestMethod]
        public void MultipleNamespaces()
        {
            var unit = GetUnit("MultipleNamespaces.cs.test");
            var methodDeclaration = unit.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            string qualifiedName = QualifiedNameResolver.Resolve(methodDeclaration);
            
            Assert.AreEqual("BankingApp.TestFiles.MultipleNamespaces", qualifiedName);
        }

        [TestMethod]
        public void NestedTypes()
        {
            var unit = GetUnit("NestedTypes.cs.test");
            var methodDeclaration = unit.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            string qualifiedName = QualifiedNameResolver.Resolve(methodDeclaration);

            Assert.AreEqual("BankingApp.TestFiles.Container+Nest", qualifiedName);
        }

        [TestMethod]
        public void WithoutNamespace()
        {
            var unit = GetUnit("WithoutNamespace.cs.test");
            var methodDeclaration = unit.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            string qualifiedName = QualifiedNameResolver.Resolve(methodDeclaration);

            Assert.AreEqual("WithoutNamespace", qualifiedName);
        }

        [TestMethod]
        public void NestedTypesWithoutNamespace()
        {
            var unit = GetUnit("NestedTypesWithoutNamespace.cs.test");
            var methodDeclaration = unit.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            string qualifiedName = QualifiedNameResolver.Resolve(methodDeclaration);

            Assert.AreEqual("Container+Nest", qualifiedName);
        }

        [TestMethod]
        public void OutsideClassDeclaration()
        {
            var unit = GetUnit("WithoutNamespace.cs.test");

            string qualifiedName = QualifiedNameResolver.Resolve(unit);

            Assert.AreEqual("", qualifiedName);
        }

        [TestMethod]
        public void StringLiteralToken()
        {
            var unit = GetUnit("WithoutNamespace.cs.test");
            var stringLiteralToken = unit.DescendantTokens()
                .Where(n => n.Kind() == SyntaxKind.StringLiteralToken)
                .First();

            string qualifiedName = QualifiedNameResolver.Resolve(stringLiteralToken);

            Assert.AreEqual("WithoutNamespace", qualifiedName);

        }

        private CompilationUnitSyntax GetUnit(string fileName)
        {
            string path = Path.Combine("TestFiles", "QualifiedNameResolver", fileName);
            string text = File.ReadAllText(path);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            return tree.GetCompilationUnitRoot();
        }
    }
}
