using JmcModLib.Core;
using System;
using System.Linq;

namespace DuckSort.Core
{
    internal static class JmcModLibLinker
    {
        static bool hasLib = false;
        public static void Setup()
        {
            hasLib = AppDomain.CurrentDomain.GetAssemblies()
                                            .Any(a => string.Equals(a.GetName().Name, "JmcModLib", StringComparison.OrdinalIgnoreCase));
            if (hasLib)
            {
                JmcModLib.Utils.ModLogger.Debug("Setup");
                ModRegistry.Register(true, VersionInfo.modinfo, VersionInfo.Name, VersionInfo.Version)?
                           .RegisterL10n()
                           .Done();
                JmcModLib.Utils.ModLogger.Debug("退出Setup");
            }
            else
            {
                DuckSort.Utils.ModLogger.Warn("未加载JmcModLib!");
            }
        }
    }
}
