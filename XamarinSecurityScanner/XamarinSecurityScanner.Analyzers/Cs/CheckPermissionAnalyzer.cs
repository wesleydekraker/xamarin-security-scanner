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
    internal class CheckPermissionAnalyzer : CsAnalyzer
    {
        // From /qark/plugins/generic/check_permissions.py, under Apache License, Version 2.0.
        private static readonly string[] PermissionChecks = {
            "CheckCallingOrSelfPermission",
            "CheckCallingOrSelfUriPermission",
            "CheckPermission",
            "EnforceCallingOrSelfPermission",
            "EnforceCallingOrSelfUriPermission",
            "EnforcePermission",
        };

        public override void Analyze(CsFile csFile)
        {
            var invocations = csFile.GetUnit().DescendantNodes().OfType<InvocationExpressionSyntax>();

            var vulnerabilities = invocations
                    .Where(IsPermissionCheck)
                    .Select(invocation =>
                        new Vulnerability
                        {
                            Code = "CheckPermission",
                            Title = "Permissions may not be enforced",
                            Description = $"Permissions may not be enforced when using this method in an exported component: {GetMethodName(invocation)}(...).",
                            FilePath = csFile.FilePath,
                            FullyQualifiedName = QualifiedNameResolver.Resolve(invocation),
                            LineNumber = invocation.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                        }).ToList();

            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }

        private static bool IsPermissionCheck(InvocationExpressionSyntax invocation)
        {
            return GetMethodName(invocation) != null;
        }
        
        private static string GetMethodName(InvocationExpressionSyntax invocation)
        {
            ExpressionSyntax expression = invocation.Expression;

            string methodName;
            switch (expression)
            {
                case MemberAccessExpressionSyntax memberAccess:
                    methodName = memberAccess.Name.ToString();
                    break;
                case IdentifierNameSyntax identifierName:
                    methodName = identifierName.ToString();
                    break;
                default:
                    return null;
            }
            
            return PermissionChecks.FirstOrDefault(permissionCheck => methodName == permissionCheck);
        }
        
    }
}
