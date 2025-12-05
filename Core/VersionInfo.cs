using Duckov.Modding;

namespace DuckSort.Core
{
    public static class VersionInfo
    {
        public const string Name = "DuckSort";
        public const string Version = "1.8.1";
        public static string Tag => $"[{Name} v{Version}]";
        public static ModInfo modinfo;
    }
}