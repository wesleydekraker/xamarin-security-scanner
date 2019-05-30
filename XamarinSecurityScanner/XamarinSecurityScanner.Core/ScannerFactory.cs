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

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Text;

[assembly: InternalsVisibleTo("XamarinSecurityScanner.Test")]
namespace XamarinSecurityScanner.Core
{    
    public class ScannerFactory : IScannerFactory
    {
        public List<ICsAnalyzer> CsAnalyzers { get; set; }
        public List<IAndroidManifestAnalyzer> AndroidManifestAnalyzers { get; set; }
        public List<ITextAnalyzer> TextAnalyzers { get; set; }

        public IScanner Create()
        {
            if (CsAnalyzers == null)
            {
                throw new XamarinSecurityScannerException($"The property \"CsAnalyzers\" cannot be null in ScannerFactory.");
            }

            if (AndroidManifestAnalyzers == null)
            {
                throw new XamarinSecurityScannerException($"The property \"AndroidManifestAnalyzers\" cannot be null in ScannerFactory.");
            }

            if (TextAnalyzers == null)
            {
                throw new XamarinSecurityScannerException($"The property \"TextAnalyzers\" cannot be null in ScannerFactory.");
            }

            return new Scanner
            {
                FileFinder = new FileFinder(),
                CsAnalyzers = CsAnalyzers,
                AndroidManifestAnalyzers = AndroidManifestAnalyzers,
                TextAnalyzers = TextAnalyzers
            };
        }
    }
}
