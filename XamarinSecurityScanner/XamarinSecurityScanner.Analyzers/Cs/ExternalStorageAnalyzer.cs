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
    internal class ExternalStorageAnalyzer : CsAnalyzer
    {
        // From /qark/plugins/file/external_storage.py, under Apache License, Version 2.0.
        private static readonly string[] ExternalStorageMethods = {
            "GetExternalFilesDir",
            "GetExternalFilesDirs",
            "GetExternalMediaDirs",
            "GetExternalCacheDirs",
        };

        public override void Analyze(CsFile csFile)
        {
            var invocations = csFile.GetUnit().DescendantNodes().OfType<InvocationExpressionSyntax>();

            var expressions = invocations.Select(expression => expression.Expression);
            
            var vulnerabilities = expressions
                .Where(IsExternalStorageMethod)
                .Select(expression => new Vulnerability
                {
                    Code = "ExternalStorage",
                    Title = "External storage is used",
                    Description = $"Files on external storage can be accessed by any app. Check this method: {GetMethodName(expression)}(...).",
                    FilePath = csFile.FilePath,
                    FullyQualifiedName = QualifiedNameResolver.Resolve(expression),
                    LineNumber = expression.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }
        
        private static bool IsExternalStorageMethod(ExpressionSyntax expression)
        {
            return ExternalStorageMethods.Any(method => expression.ToString().Contains(method));
        }
        
        private static string GetMethodName(ExpressionSyntax expression)
        {
            return ExternalStorageMethods.First(method => expression.ToString().Contains(method));
        }
    }
}
