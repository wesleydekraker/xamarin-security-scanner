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
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace XamarinSecurityScanner.Analyzers.Cs
{
    internal class JavaScriptEnabledAnalyzer : CsAnalyzer
    {
        // From /qark/plugins/webview/javascript_enabled.py, under Apache License, Version 2.0.
        private readonly string _javaScriptEnabledProperty = "JavaScriptEnabled";

        public override void Analyze(CsFile csFile)
        {
            var assignments = csFile.GetUnit().DescendantNodes().OfType<AssignmentExpressionSyntax>();

            var accessExpressions = assignments
                .Where(expression => expression.Left is MemberAccessExpressionSyntax)
                .Select(expression => (MemberAccessExpressionSyntax) expression.Left);
            
            var vulnerabilities = accessExpressions
                .Where(expression => expression.Name.ToString() == _javaScriptEnabledProperty)
                .Select(expression => new Vulnerability
                {
                    Code = "JavaScriptEnabled",
                    Title = "JavaScript enabled in WebView",
                    Description = $"Enabling JavaScript in a WebView opens the door to XSS attacks.",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(expression),
                    LineNumber = expression.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }
    }
}
