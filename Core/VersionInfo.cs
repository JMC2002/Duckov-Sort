using System;
using System.Collections.Generic;
using System.Text;

namespace DuckSort.Core
{
    public static class VersionInfo
    {
        public const string Name = "DuckSort";
        public const string Version = "1.2.3";

        public static string Tag => $"[{Name} v{Version}]";
    }
}