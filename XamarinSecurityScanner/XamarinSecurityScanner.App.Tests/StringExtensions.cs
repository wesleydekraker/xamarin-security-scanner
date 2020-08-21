using System;

namespace XamarinSecurityScanner.App.Tests
{
    public static class StringExtensions
    {
        public static string NormalizeEndOfLine(this String str)
        {
            return str
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }
    }
}
