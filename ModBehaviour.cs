using Duckov.UI;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using System.Reflection;
using TMPro;
using UnityEngine;
using DuckSort.UI;

namespace DuckSort
{
    using Core;
    using DuckSort.Utils;
    using System;

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private readonly string HarmonyId = $"{VersionInfo.Name}.CustomSortButtons";
        private Harmony? _harmony;

        private AddText addText = new();

        void Awake()
        {
            ModLogger.Info("模组已加载");
        }
        void OnEnable()
        {
            addText.Enable();
            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModLogger.Info("Harmony 补丁已加载");
        }

        void OnDestroy()
        {
            addText.Disable();
        }

        void OnDisable()
        {
            _harmony?.UnpatchAll(HarmonyId);
            ModLogger.Info("Harmony 补丁已卸载");
        }
    }
}
