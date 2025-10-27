using System;
using UnityEngine;
using DuckSort.Core;
using DuckSort.Utils;

namespace DuckSort.Core
{
    /// <summary>
    /// 将 ModConfigAPI 与本地 ModConfig.json 绑定
    /// </summary>
    public static class ModConfigLinker
    {
        private static bool initialized = false;
        private const string ModName = VersionInfo.Name;

        public static void Init()
        {
            if (initialized) return;
            initialized = true;

            if (!ModConfigAPI.IsAvailable())
            {
                ModLogger.Warn("ModConfig 未检测到，将仅使用本地配置文件。");
                return;
            }

            ModLogger.Info("检测到 ModConfig，正在注册配置项...");

            // === 注册布尔配置项 ===
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowPriceButton),      L10n.GetLabel("显示价格按钮"),   ModConfig.ShowPriceButton);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowWeightButton),     L10n.GetLabel("显示重量按钮"),   ModConfig.ShowWeightButton);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowRatioButton),      L10n.GetLabel("显示价重比按钮"), ModConfig.ShowRatioButton);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowQualityButton),    L10n.GetLabel("显示稀有度按钮"), ModConfig.ShowQualityButton);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowUnitPriceButton),  L10n.GetLabel("显示单价按钮"),   ModConfig.ShowUnitPriceButton);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowPriceText),        L10n.GetLabel("显示价格信息"),   ModConfig.ShowPriceText);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.ShowRatioText),        L10n.GetLabel("显示价重比信息"), ModConfig.ShowRatioText);
            ModConfigAPI.SafeAddBoolDropdownList(ModName, nameof(ModConfig.DefaultSortAscending), L10n.GetLabel("是否升序排序"),   ModConfig.DefaultSortAscending);

            // === 从 ModConfig API 载入已保存的设置（覆盖本地） ===
            SyncFromModConfigAPI();

            // === 注册事件回调 ===
            ModConfigAPI.SafeAddOnOptionsChangedDelegate(OnOptionChanged);

            ModLogger.Info("ModConfigLinker 初始化完成。");
        }

        /// <summary>
        /// 当游戏内配置被更改时回调
        /// </summary>
        private static void OnOptionChanged(string key)
        {
            if (!key.StartsWith(ModName)) return;

            string shortKey = key.Substring(ModName.Length + 1);
            ModLogger.Info($"ModConfig 选项已修改: {shortKey}");

            switch (shortKey)
            {
                case nameof(ModConfig.ShowPriceButton):
                    ModConfig.ShowPriceButton = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowPriceButton);
                    break;
                case nameof(ModConfig.ShowWeightButton):
                    ModConfig.ShowWeightButton = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowWeightButton);
                    break;
                case nameof(ModConfig.ShowRatioButton):
                    ModConfig.ShowRatioButton = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowRatioButton);
                    break;
                case nameof(ModConfig.ShowQualityButton):
                    ModConfig.ShowQualityButton = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowQualityButton);
                    break;
                case nameof(ModConfig.ShowUnitPriceButton):
                    ModConfig.ShowUnitPriceButton = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowUnitPriceButton);
                    break;
                case nameof(ModConfig.ShowPriceText):
                    ModConfig.ShowPriceText = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowPriceText);
                    break;
                case nameof(ModConfig.ShowRatioText):
                    ModConfig.ShowRatioText = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowRatioText);
                    break;
                case nameof(ModConfig.DefaultSortAscending):
                    ModConfig.DefaultSortAscending = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.DefaultSortAscending);
                    break;
            }

            // 保存到本地 JSON
            ModConfig.Save();
        }

        /// <summary>
        /// 从 ModConfig API 载入（用于初始同步）
        /// </summary>
        public static void SyncFromModConfigAPI()
        {
            if (!ModConfigAPI.IsAvailable()) return;

            ModConfig.ShowPriceButton = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowPriceButton), ModConfig.ShowPriceButton);
            ModConfig.ShowWeightButton = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowWeightButton), ModConfig.ShowWeightButton);
            ModConfig.ShowRatioButton = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowRatioButton), ModConfig.ShowRatioButton);
            ModConfig.ShowPriceText = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowPriceText), ModConfig.ShowPriceText);
            ModConfig.ShowRatioText = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowRatioText), ModConfig.ShowRatioText);
            ModConfig.DefaultSortAscending = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.DefaultSortAscending), ModConfig.DefaultSortAscending);

            // 同步到本地文件
            ModConfig.Save();
        }
    }
}
