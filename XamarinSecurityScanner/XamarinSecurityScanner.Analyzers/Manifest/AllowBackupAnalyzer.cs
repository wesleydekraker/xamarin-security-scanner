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

using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Models;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XamarinSecurityScanner.Analyzers.Manifest
{
    internal class AllowBackupAnalyzer : AndroidManifestAnalyzer
    {
        // From /qark/plugins/manifest/allow_backup.py, under Apache License, Version 2.0.
        private readonly XName _allowBackup = XName.Get("allowBackup", "http://schemas.android.com/apk/res/android");

        public override void Analyze(AndroidManifestFile androidManifestFile)
        {
            var vulnerabilities = androidManifestFile.GetXElement()
                .Elements("application")
                .Where(IsBackupAllowed)
                .Select(e => new Vulnerability
                {
                    Code = "AllowBackup",
                    Title = "Backups are enabled",
                    SeverityLevel = SeverityLevel.Medium,
                    Description = $"Enabling backups may leak sensitive data to the cloud.",
                    FilePath = androidManifestFile.FilePath,
                    FullyQualifiedName = "AndroidManifest.xml",
                    LineNumber = ((IXmlLineInfo)e).LineNumber
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }

        private bool IsBackupAllowed(XElement element)
        {
            return element.Attributes(_allowBackup).All(a => a.Value != "false");
        }
    }
}
