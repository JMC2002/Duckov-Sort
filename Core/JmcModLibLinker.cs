using DuckSort.Utils;
using JmcModLib.Core;
using JmcModLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Unity.VisualScripting;

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
                ModRegistry.Register(VersionInfo.Name, VersionInfo.Version);
                JmcModLib.Utils.L10n.Register();
                JmcModLib.Utils.ModLogger.Debug("退出Setup");
            }
            else
            {
                DuckSort.Utils.ModLogger.Warn("未加载JmcModLib!");
            }
        }
    }
}
