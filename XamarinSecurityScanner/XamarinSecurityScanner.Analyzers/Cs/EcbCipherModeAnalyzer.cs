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
    // Inspired by /qark/plugins/crypto/ecb_cipher_usage.py, under Apache License, Version 2.0.
    internal class EcbCipherModeAnalyzer : CsAnalyzer
    {
        public override void Analyze(CsFile csFile)
        {
            var assignments = csFile.GetUnit().DescendantNodes().OfType<AssignmentExpressionSyntax>();

            var vulnerabilities = assignments
                .Where(IsEcbCipherMode)
                .Select(assignment => new Vulnerability
                {
                    Code = "EcbCipherMode",
                    Title = "Unsafe cipher mode used",
                    SeverityLevel = SeverityLevel.High,
                    Description = $"You may leak information by using the ECB cipher mode. Encrypting the same block of bits using this mode returns the same output.",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(assignment),
                    LineNumber = assignment.Right.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }
        
        private static bool IsEcbCipherMode(AssignmentExpressionSyntax assignment)
        {
            if (!(assignment.Right is MemberAccessExpressionSyntax memberAccess))
            {
                return false;
            }

            ExpressionSyntax className = memberAccess.Expression;
            
            switch (className)
            {
                case MemberAccessExpressionSyntax fullyQualifiedName:
                    className = fullyQualifiedName.Name;
                    break;
                case IdentifierNameSyntax identifierName:
                    className = identifierName;
                    break;
                default:
                    return false;
            }
            
            return className.ToString() == "CipherMode" && memberAccess.Name.ToString() == "ECB";
        }
    }
}
