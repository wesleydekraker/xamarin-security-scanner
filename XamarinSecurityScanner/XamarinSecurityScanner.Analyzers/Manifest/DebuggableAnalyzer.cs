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
    internal class DebuggableAnalyzer : AndroidManifestAnalyzer
    {
        // From /qark/plugins/manifest/debuggable.py, under Apache License, Version 2.0.
        private readonly XName _debuggable = XName.Get("debuggable", "http://schemas.android.com/apk/res/android");

        public override void Analyze(AndroidManifestFile androidManifestFile)
        {
            var vulnerabilities = androidManifestFile.GetXElement()
                .Elements("application")
                .Where(IsDebuggable)
                .Select(e => new Vulnerability
                {
                    Code = "Debuggable",
                    Title = "App has debugging enabled",
                    SeverityLevel = SeverityLevel.Medium,
                    Description = "Enabling debugging makes it easier for an attacker to reverse engineer your app.",
                    FilePath = androidManifestFile.FilePath,
                    FullyQualifiedName = "AndroidManifest.xml",
                    LineNumber = ((IXmlLineInfo)e).LineNumber
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }

        private bool IsDebuggable(XElement element)
        {
            return element.Attributes(_debuggable)
                .Any(a => a.Value == "true");
        }
    }
}
