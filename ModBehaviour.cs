using DuckSort.Core;
using DuckSort.Patches;
using DuckSort.UI;
using DuckSort.Utils;
using SodaCraft.Localizations;
using System;
using UnityEngine;

namespace DuckSort
{

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public AddText addText = new();

        private HarmonyHelper harmonyHelper = new("CustomSortButtons");
        protected override void OnAfterSetup()
        {
            ModLogger.Info("模组已启用");
            ModConfig.Load();
            Core.VersionInfo.modinfo = info;
            Core.JmcModLibLinker.Setup();
            ModSettingLinker.Init(info);
            ModConfigLinker.Init();

            addText.Enable();
            harmonyHelper.OnEnable();
        }

        protected virtual void OnBeforeDeactivate()
        {
            ModConfig.Save();
            ModSettingLinker.Dispose();
            ModConfigLinker.Dispose();
            addText.Disable();
            harmonyHelper.OnDisable();
            ModLogger.Info("Mod 已禁用，配置已保存");
        }
    }
}
