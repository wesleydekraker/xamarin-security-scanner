﻿/*
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
    // Inspired by /qark/plugins/cert/cert_validation_methods_overriden.py, under Apache License, Version 2.0.
    internal class CertificateValidationAnalyzer : CsAnalyzer
    {
        private const string Callback = "ServicePointManager.ServerCertificateValidationCallback";

        public override void Analyze(CsFile csFile)
        {
            var accessExpressions = csFile.GetUnit().DescendantNodes().OfType<MemberAccessExpressionSyntax>();

            var vulnerabilities = accessExpressions
                .Where(expression => expression.ToString().Contains(Callback))
                .Select(expression => new Vulnerability
                {
                    Code = "CertificateValidation",
                    Title = "Certificate validation overwritten",
                    SeverityLevel = SeverityLevel.High,
                    Description = $"Certificate validation is overwritten. Incorrectly implementing this validation may open the door to man-in-the-middle attacks. Please check your implementation to see if your vulnerable (e.g. always returning true).",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(expression),
                    LineNumber = expression.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }
    }
}
