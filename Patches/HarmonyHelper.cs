using DuckSort.Core;
using DuckSort.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.Rendering;

namespace DuckSort.Patches
{
    public class HarmonyHelper
    {
        private string PatchId;
        private string PatchTag => $"{VersionInfo.Name}.{PatchId}";
        private Harmony? _harmony;

        public HarmonyHelper(string patchId) => PatchId = patchId;

        public void OnEnable()
        {
            _harmony = new Harmony(PatchTag);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModLogger.Info($"Harmony 补丁{PatchId}已加载");
        }

        public void OnDisable()
        {
            _harmony?.UnpatchAll(PatchTag);
            ModLogger.Info($"Harmony 补丁{PatchId}已卸载");
        }
    }
}
