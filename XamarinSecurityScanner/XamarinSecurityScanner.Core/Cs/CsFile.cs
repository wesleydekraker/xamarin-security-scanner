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

using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace XamarinSecurityScanner.Core.Cs
{
    public class CsFile
    {
        public string FilePath { get; }

        public CsFile(string fileName)
        {
            FilePath = fileName;
        }

        public CompilationUnitSyntax GetUnit()
        {
            var text = "";
            try
            {
                text = File.ReadAllText(FilePath);
            }
            catch (IOException)
            {
                XamarinSecurityScannerLogger.Log("Could not read file {0}.", FilePath);
            }

            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            return tree.GetCompilationUnitRoot();
        }
    }
}