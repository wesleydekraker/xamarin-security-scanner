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
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis;
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core;

namespace XamarinSecurityScanner.Analyzers.Cs
{
    // Inspired by /qark/plugins/file/http_url_hardcoded.py, under Apache License, Version 2.0.
    internal class HardcodedHttpUrlAnalyzer : CsAnalyzer
    {
        private static readonly string[] IgnoreUrls =
        {
                "http://xamarin.com",
                "http://schemas.microsoft.com"
        };
        
        public override void Analyze(CsFile csFile)
        {
            var stringLiteralTokens = csFile.GetUnit().DescendantTokens()
                .Where(n => n.Kind() == SyntaxKind.StringLiteralToken);

            var vulnerabilities = stringLiteralTokens
                .Where(IsHttpUrl)
                .Where(IsNotIgnored)
                .Select(literal => new Vulnerability
                {
                    Code = "HardcodedHttpUrl",
                    Title = "Hardcoded HTTP URL found",
                    SeverityLevel = SeverityLevel.Low,
                    Description = $"HTTP traffic may not be encrypted, which opens the door to man-in-the-middle attacks. HTTP URL: {literal.ValueText}.",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(literal),
                    LineNumber = literal.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();

            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }

        private static bool IsHttpUrl(SyntaxToken literal)
        {
            string valueText = literal.ValueText;
            return valueText.StartsWith("http://");
        }

        private static bool IsNotIgnored(SyntaxToken literal)
        {
            string valueText = literal.ValueText;
            return !IgnoreUrls.Any(valueText.StartsWith);
        }
    }
}
