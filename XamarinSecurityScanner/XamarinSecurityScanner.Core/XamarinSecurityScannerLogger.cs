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

namespace XamarinSecurityScanner.Core
{
    public static class XamarinSecurityScannerLogger
    {
        public static Action<string> LogEvent { get; set; }

        public static void Log(string message)
        {
            LogEvent?.Invoke(message);
        }

        public static void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }
    }
}