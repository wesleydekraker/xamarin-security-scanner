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

using System;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Models;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using XamarinSecurityScanner.Core;

namespace XamarinSecurityScanner.Analyzers.Manifest
{
    internal class MinSdkAnalyzer : AndroidManifestAnalyzer
    {
        // From /qark/plugins/manifest/min_sdk.py, under Apache License, Version 2.0.
        private readonly XName _minSdkVersion = XName.Get("minSdkVersion", "http://schemas.android.com/apk/res/android");

        public override void Analyze(AndroidManifestFile androidManifestFile)
        {
            var vulnerabilities = androidManifestFile.GetXElement()
                .Elements("uses-sdk")
                .Where(IsOutdated)
                .Select(e => new Vulnerability
                {
                    Code = "MinSdk",
                    Title = "App supports outdated Android version",
                    Description = "Apps should no longer support Android Gingerbread or lower. This version is used by less than 0.3% of all devices and the latest release was in 2011.",
                    FilePath = androidManifestFile.FilePath,
                    FullyQualifiedName = "AndroidManifest.xml",
                    LineNumber = ((IXmlLineInfo)e).LineNumber
                }).ToList();
            
            vulnerabilities.ForEach(OnVulnerabilityDiscovered);
        }

        private bool IsOutdated(XElement element)
        {
            XAttribute minSdkVersion = element.Attributes(_minSdkVersion).FirstOrDefault();

            if (minSdkVersion == null)
            {
                return false;
            }

            try
            {
                int version = int.Parse(minSdkVersion.Value);
                return version < 11;
            }
            catch (FormatException)
            {
                XamarinSecurityScannerLogger.Log("Could not parse minSdkVersion to number.");
                return false;
            }
            
            
        }
    }
}
