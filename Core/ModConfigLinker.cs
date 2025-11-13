using Duckov.Modding;
using DuckSort.Utils;
using System;
using System.Linq.Expressions;

namespace DuckSort.Core
{
    /// <summary>
    /// 将 ModConfigAPI 与本地 ModConfig.json 绑定
    /// </summary>
    public static class ModConfigLinker
    {
        private static bool initialized = false;
        private const string ModName = VersionInfo.Name;

        public static bool AddBoolDropdownList(Expression<Func<bool>> expr, string description, string? key = null)
        {
            try
            {
                return ModConfigAPI.SafeAddBoolDropdownList(
                        ModName,
                        expr.Body is MemberExpression m ? m.Member.Name : "unknown",
                        L10n.GetLabel(description),
                        expr.Compile().Invoke());
            }
            catch (Exception ex)
            {
                ModLogger.Error($"添加按钮\"{description}\"时出现问题", ex);
                return false;
            }
        }

        public static void Init()
        {
            if (initialized) return;
            // 首次尝试初始化，若Config在这个MOD前加载会触发
            ModConfigAPI.Initialize();
            // 当任意 Mod 启用时尝试与 ModConfig 连接
            ModManager.OnModActivated += TryInitModConfig;
            initialized = true;
        }

        public static void Dispose()
        {
            initialized = false;
            ModManager.OnModActivated -= TryInitModConfig;
        }

        public static void TryInitModConfig(ModInfo info, Duckov.Modding.ModBehaviour behaviour)
        {
            if (info.name != ModConfigAPI.ModConfigName || !ModConfigAPI.Initialize())
                return;

            if (!ModConfigAPI.IsAvailable())
            {
                ModLogger.Warn("ModConfig 未检测到，将仅使用本地配置文件。");
                return;
            }

            ModLogger.Info("检测到 ModConfig，正在注册配置项...");

            // === 注册布尔配置项 ===
            AddBoolDropdownList(() => ModConfig.ShowPriceButton,      "显示价格按钮");
            AddBoolDropdownList(() => ModConfig.ShowWeightButton,     "显示重量按钮");
            AddBoolDropdownList(() => ModConfig.ShowRatioButton,      "显示价重比按钮");
            AddBoolDropdownList(() => ModConfig.ShowQualityButton,    "显示稀有度按钮");
            AddBoolDropdownList(() => ModConfig.ShowUnitPriceButton,  "显示单价按钮");
            AddBoolDropdownList(() => ModConfig.ShowIDButton,         "显示ID按钮");
            AddBoolDropdownList(() => ModConfig.ShowPriceText,        "显示价格信息");
            AddBoolDropdownList(() => ModConfig.ShowRatioText,        "显示价重比信息");
            AddBoolDropdownList(() => ModConfig.ShowUnitPriceText,    "显示单价信息");
            AddBoolDropdownList(() => ModConfig.DefaultSortAscending, "交换左右键排序方向");
            AddBoolDropdownList(() => ModConfig.EnableDebugLogs,      "启用调试日志");

            // === 从 ModConfig API 载入已保存的设置（覆盖本地） ===
            // SyncFromModConfigAPI();

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
                case nameof(ModConfig.ShowIDButton):
                    ModConfig.ShowIDButton = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowIDButton);
                    break;
                case nameof(ModConfig.ShowPriceText):
                    ModConfig.ShowPriceText = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowPriceText);
                    break;
                case nameof(ModConfig.ShowRatioText):
                    ModConfig.ShowRatioText = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowRatioText);
                    break;
                case nameof(ModConfig.ShowUnitPriceText):
                    ModConfig.ShowUnitPriceText = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.ShowUnitPriceText);
                    break;
                case nameof(ModConfig.DefaultSortAscending):
                    ModConfig.DefaultSortAscending = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.DefaultSortAscending);
                    break;
                case nameof(ModConfig.EnableDebugLogs):
                    ModConfig.EnableDebugLogs = ModConfigAPI.SafeLoad(ModName, shortKey, ModConfig.EnableDebugLogs);
                    break;
            }

            // 保存到本地 JSON
            ModConfig.Save();

            // 通知所有订阅者（AddText、HarmonyHelper等）
            ModConfig.InvokeConfigChanged();
        }

        /// <summary>
        /// 从 ModConfig API 载入（用于初始同步）
        /// </summary>
        public static void SyncFromModConfigAPI()
        {
            if (!ModConfigAPI.IsAvailable()) return;
            ModLogger.Info("从 ModConfigAPI 同步配置项...");

            ModConfig.ShowPriceButton = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowPriceButton), ModConfig.ShowPriceButton);
            ModConfig.ShowWeightButton = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowWeightButton), ModConfig.ShowWeightButton);
            ModConfig.ShowRatioButton = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowRatioButton), ModConfig.ShowRatioButton);
            ModConfig.ShowPriceText = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowPriceText), ModConfig.ShowPriceText);
            ModConfig.ShowRatioText = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.ShowRatioText), ModConfig.ShowRatioText);
            ModConfig.DefaultSortAscending = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.DefaultSortAscending), ModConfig.DefaultSortAscending);
            ModConfig.EnableDebugLogs = ModConfigAPI.SafeLoad(ModName, nameof(ModConfig.EnableDebugLogs), ModConfig.EnableDebugLogs);

            // 同步到本地文件
            ModConfig.Save();
            ModLogger.Info("同步完成。");
        }
    }
}
