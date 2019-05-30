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
    // Inspired by /qark/plugins/file/android_logging.py, under Apache License, Version 2.0.
    internal class LoggingAnalyzer : CsAnalyzer
    {
        private static readonly string[] LogMethods = {
            "Verbose",
            "Debug",
            "Info",
            "Warn",
            "Error",
            "Wtf",
            "WriteLine",
        };

        public override void Analyze(CsFile csFile)
        {
            var accessExpressions = csFile.GetUnit().DescendantNodes().OfType<MemberAccessExpressionSyntax>();

            var vulnerabilities = accessExpressions
                .Where(IsLogMethod)
                .Select(expression => new Vulnerability
                {
                    Code = "Logging",
                    Title = "Logging was found",
                    Description = $"Logging was found in the app: Log.{GetMethodName(expression)}(...). Other apps may read the logs.",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(expression),
                    LineNumber = expression.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();

            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }

        private static bool IsLogMethod(MemberAccessExpressionSyntax memberAccess)
        {
            return GetMethodName(memberAccess) != null;
        }
        
        private static string GetMethodName(MemberAccessExpressionSyntax memberAccess)
        {
            ExpressionSyntax expression = memberAccess.Expression;
            string className = null;
            
            switch (expression)
            {
                case MemberAccessExpressionSyntax fullyQualifiedName:
                    className = fullyQualifiedName.Name.ToString();
                    break;
                case IdentifierNameSyntax identifierName:
                    className = identifierName.ToString();
                    break;
            }

            if (className != "Log")
            {
                return null;
            }

            var methodName = memberAccess.Name.ToString();
            return LogMethods.FirstOrDefault(logMethod => methodName == logMethod);
        }
    }
}
