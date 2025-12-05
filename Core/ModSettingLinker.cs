using Duckov.Modding;
using DuckSort.Utils;
using System;
using System.Linq.Expressions;
using UnityEngine;

namespace DuckSort.Core
{
    /// <summary>
    /// 负责将 DuckSort 的 ModConfig 注册到 ModSetting。
    /// </summary>
    public static class ModSettingLinker
    {
        private static bool _initialized = false;

        /// <summary>
        /// 构建带保存逻辑的封装器，用于注册回调事件，回调时会通知 <c>ModConfig.InvokeConfigChanged()</c>;
        /// </summary>
        /// <typeparam name="T">目标变量的类型</typeparam>
        /// <param name="setter">目标变量的setter函数</param>
        /// <param name="onValueChange">触发回调时的额外逻辑，可留空</param>
        /// <returns>返回一个用于回调的事件</returns>
        private static Action<T> BuildSaveWrapper<T>(Action<T> setter, Action<T>? onValueChange = null)
        {
            // 返回最终回调
            return (T val) =>
            {
                setter(val);
                onValueChange?.Invoke(val);

                ModConfig.Save();
                ModConfig.InvokeConfigChanged();
            };
        }

        /// <summary>
        /// 添加了一个切换设置，该设置可与布尔属性或字段绑定，并且支持针对值变化的可选回调函数。
        /// </summary>
        /// <remarks>该切换开关的初始值取自绑定的属性或字段。当切换开关的值发生变化时，它会更新绑定的成员。如果提供了回调函数，那么在值更新后该回调函数将会被调用。</remarks>
        /// <param name="expr">标识要绑定的bool属性或字段的表达式，形如 <c> () => obj.field </c></param>
        /// <param name="description">用于显示的描述文本，将自动调用本地化</param>
        /// <param name="onValueChange">一个可选的回调函数，在切换值发生变化时会被调用。该函数会接收新的值作为参数。</param>
        /// <param name="key">切换设置的可选唯一键。如果未指定，则根据成员名称生成键。</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool AddToggle(Expression<Func<bool>> expr, string description, Action<bool>? onValueChange = null, string? key = null)
        {
            try
            {
                var (g, s) = ExprHelper.GetOrCreateAccessors(expr);
                return ModSettingAPI.AddToggle(
                        expr.Body is MemberExpression m ? m.Member.Name : "unknown",
                        L10n.GetLabel(description),
                        g(),
                        BuildSaveWrapper(s, onValueChange));
            }
            catch (Exception ex)
            {
                ModLogger.Error($"添加按钮\"{description}\"时出现问题", ex);
                return false;
            }
        }

        public static void Init(ModInfo info)
        {
            if (_initialized) return;
            if (ModSettingAPI.Init(info)) RegisterSettings();
            // 当任意 Mod 启用时尝试与 ModSetting 连接
            ModManager.OnModActivated += TryInitModSetting;
            JmcModLib.Utils.L10n.LanguageChanged += OnLangChanged;
            _initialized = true;
        }

        public static void Dispose()
        {
            JmcModLib.Utils.L10n.LanguageChanged -= OnLangChanged;
            ModManager.OnModActivated -= TryInitModSetting;
            ModSettingAPI.RemoveMod();
            _initialized = false;
        }

        private static void OnLangChanged(SystemLanguage lang)
        {
            ModSettingAPI.RemoveMod();
            ModSettingLinker.RegisterSettings();
        }

        private static void TryInitModSetting(ModInfo info, Duckov.Modding.ModBehaviour behaviour)
        {
            ModLogger.Debug($"检测到Mod {info.name}启用");
            // 只在 ModSetting 启动时进行初始化
            if (info.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(VersionInfo.modinfo))
                return;

            ModLogger.Info("检测到 ModSetting 启用，尝试注册配置界面");

            RegisterSettings();
        }



        public static void RegisterSettings()
        {
            try
            {
                // 添加各个 UI 控件
                AddToggle(() => ModConfig.ShowPriceButton,      "显示价格按钮");
                AddToggle(() => ModConfig.ShowWeightButton,     "显示重量按钮");
                AddToggle(() => ModConfig.ShowRatioButton,      "显示价重比按钮");
                AddToggle(() => ModConfig.ShowQualityButton,    "显示稀有度按钮");
                AddToggle(() => ModConfig.ShowUnitPriceButton,  "显示单价按钮");
                AddToggle(() => ModConfig.ShowIDButton,         "显示ID按钮");
                AddToggle(() => ModConfig.ShowPriceText,        "显示价格信息");
                AddToggle(() => ModConfig.ShowRatioText,        "显示价重比信息");
                AddToggle(() => ModConfig.ShowUnitPriceText,    "显示单价信息");
                AddToggle(() => ModConfig.DefaultSortAscending, "交换左右键排序方向");
                AddToggle(() => ModConfig.EnableDebugLogs,      "启用调试日志");

                ModLogger.Info("成功注册 ModSetting UI");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"注册 ModSetting UI 时出错", ex);
            }
        }
    }
}
