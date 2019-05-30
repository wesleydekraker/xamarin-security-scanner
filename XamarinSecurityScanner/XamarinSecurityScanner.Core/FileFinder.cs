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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using XamarinSecurityScanner.Core.Text;

namespace XamarinSecurityScanner.Core
{
    internal class FileFinder : IFileFinder
    {
        private static readonly List<string> BinaryExtensions = new List<string>
        {
            ".mp4",
            ".png",
            ".jpg",
            ".tar.gz",
            ".zip",
            ".gif",
            ".dll",
            ".apk",
        };

        internal IDirectoryWrapper DirectoryWrapper = new DirectoryWrapper();

        public ImmutableList<CsFile> GetCsFiles(string path)
        {
            return GetFiles(path, "*.cs")
                            .Select(f => new CsFile(f))
                            .ToImmutableList();
        }

        public ImmutableList<AndroidManifestFile> GetAndroidManifestFiles(string path)
        {
            return GetFiles(path, "AndroidManifest.xml")
                            .Select(f => new AndroidManifestFile(f))
                            .ToImmutableList();
        }

        public ImmutableList<TextFile> GetTextFiles(string path)
        {
            return GetFiles(path, "")
                            .Where(IsTextFile)
                            .Select(f => new TextFile(f))
                            .ToImmutableList();
        }

        private static bool IsTextFile(string path)
        {
            string extension = Path.GetExtension(path);
            return !BinaryExtensions.Contains(extension);
        }

        internal IEnumerable<string> GetFiles(string path, string searchPattern)
        {
            try
            {
                return DirectoryWrapper.GetFiles(path, searchPattern);
            }
            catch (DirectoryNotFoundException)
            {
                throw new XamarinSecurityScannerException("Incorrect scan path. Directory not found.");
            }
            catch (PathTooLongException)
            {
                throw new XamarinSecurityScannerException("Scan path is too long.");
            }
            catch (IOException)
            {
                throw new XamarinSecurityScannerException("Scan path must be a folder.");
            }
        }
    }
}
