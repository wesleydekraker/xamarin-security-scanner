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
using System.IO;
using XamarinSecurityScanner.Core.Text;
using System.Text.RegularExpressions;

namespace XamarinSecurityScanner.Analyzers.Text
{
    // Inspired by /qark/plugins/crypto/packaged_private_keys.py, under Apache License, Version 2.0.
    internal class PrivateKeyAnalyzer : TextAnalyzer
    {
        private readonly Regex _privateKeyRegex = new Regex(@"BEGIN (\w+ )?PRIVATE KEY", RegexOptions.Compiled);

        public override void Analyze(TextFile textFile)
        {
            bool isPrivateKey = _privateKeyRegex.IsMatch(textFile.GetText());
            
            if (!isPrivateKey)
            {
                return;
            }

            var vulnerability = new Vulnerability
            {
                Code = "PrivateKey",
                Title = "App contains a private key",
                Description = "Private keys should never be embedded in your app.",
                FilePath = textFile.FilePath,
                FullyQualifiedName = Path.GetFileName(textFile.FilePath),
                LineNumber = 0
            };

            OnVulnerabilityDiscovered(vulnerability);
        }
    }
}
