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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace XamarinSecurityScanner.Core
{
    public static class QualifiedNameResolver
    {
        public static string Resolve(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                return string.Empty;
            }

            var namespaceDeclarations = syntaxNode.AncestorsAndSelf()
                .OfType<NamespaceDeclarationSyntax>()
                .Reverse()
                .Select(n => n.Name.ToString())
                .ToList();

            var classDeclarations = syntaxNode.AncestorsAndSelf()
                .OfType<ClassDeclarationSyntax>()
                .Reverse()
                .Select(c => c.Identifier.ValueText)
                .ToList();

            if (namespaceDeclarations.Count == 0)
            {
                return string.Join("+", classDeclarations);
            } else
            {
                return string.Join(".", namespaceDeclarations) + "." + string.Join("+", classDeclarations);
            }
        }

        public static string Resolve(SyntaxToken syntaxToken)
        {
            SyntaxNode syntaxNode = syntaxToken.Parent;
            return Resolve(syntaxNode);
        }
    }
}
