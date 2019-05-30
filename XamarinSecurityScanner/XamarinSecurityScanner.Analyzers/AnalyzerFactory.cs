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

using XamarinSecurityScanner.Core.Cs;
using XamarinSecurityScanner.Core.Manifest;
using XamarinSecurityScanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XamarinSecurityScanner.Core.Text;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("XamarinSecurityScanner.Analyzers.Tests")]
namespace XamarinSecurityScanner.Analyzers
{
    public class AnalyzerFactory
    {
        public Assembly Assembly { private get; set; } = Assembly.GetExecutingAssembly();
        public Action<Vulnerability> VulnerabilityDiscovered { private get; set; }

        public List<ICsAnalyzer> GetCsAnalyzers()
        {
            return Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(CsAnalyzer)))
                .Select(type =>
                {
                    var analyzer = (ICsAnalyzer)Activator.CreateInstance(type);
                    analyzer.VulnerabilityDiscovered = VulnerabilityDiscovered;
                    return analyzer;
                })
                .ToList();
        }

        public List<IAndroidManifestAnalyzer> GetAndroidManifestAnalyzers()
        {
            return Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(AndroidManifestAnalyzer)))
                .Select(type =>
                {
                    var analyzer = (IAndroidManifestAnalyzer)Activator.CreateInstance(type);
                    analyzer.VulnerabilityDiscovered = VulnerabilityDiscovered;
                    return analyzer;
                })
                .ToList();
        }

        public List<ITextAnalyzer> GetTextAnalyzers()
        {
            return Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(TextAnalyzer)))
                .Select(type =>
                {
                    var analyzer = (ITextAnalyzer)Activator.CreateInstance(type);
                    analyzer.VulnerabilityDiscovered = VulnerabilityDiscovered;
                    return analyzer;
                })
                .ToList();
        }
    }
}
