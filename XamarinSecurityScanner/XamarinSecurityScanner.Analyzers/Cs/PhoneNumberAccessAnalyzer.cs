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
    internal class PhoneNumberAccessAnalyzer : CsAnalyzer
    {
        // From /qark/plugins/file/phone_identifier.py, under Apache License, Version 2.0.
        private const string PhoneNumberProperty = "Line1Number";

        public override void Analyze(CsFile csFile)
        {
            var accessExpressions = csFile.GetUnit().DescendantNodes().OfType<MemberAccessExpressionSyntax>();

            var vulnerabilities = accessExpressions
                .Where(expression => expression.Name.ToString() == PhoneNumberProperty)
                .Select(expression => new Vulnerability
                {
                    Code = "PhoneNumberAccess",
                    Title = "Access to phone number",
                    SeverityLevel = SeverityLevel.Low,
                    Description = $"Be careful accessing the phone number of your user. This is personally identifying information (PII).",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(expression),
                    LineNumber = expression.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }
    }
}
