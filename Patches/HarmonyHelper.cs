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

            // 订阅配置变化事件
            ModConfig.OnConfigChanged += OnConfigChanged;

            ModLogger.Info($"Harmony 补丁{PatchId}已加载");
        }

        public void OnDisable()
        {
            _harmony?.UnpatchAll(PatchTag);

            ModConfig.OnConfigChanged -= OnConfigChanged;

            ModLogger.Info($"Harmony 补丁{PatchId}已卸载");
        }

        private void OnConfigChanged()
        {
            ModLogger.Info("检测到配置更新，正在刷新 Harmony 补丁{PatchId}...");

            // 完全重新加载：先禁用再启用
            OnDisable();
            OnEnable();

            ModLogger.Info("Harmony 补丁{PatchId} 已根据配置重新加载完成。");
        }
    }
}
