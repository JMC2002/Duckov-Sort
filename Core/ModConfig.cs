using System;
using System.IO;
using UnityEngine;
using DuckSort.Utils;
using JmcModLib.Config;
using JmcModLib.Config.UI;

namespace DuckSort.Core
{
    public static class ModConfig
    {
        // === 私有存储字段 ===
        private static bool _showPriceButton = true;
        private static bool _showWeightButton = true;
        private static bool _showRatioButton = true;
        private static bool _showQualityButton = false;
        private static bool _showUnitPriceButton = false;
        private static bool _showIDButton = false;

        private static bool _showPriceText = true;
        private static bool _showRatioText = false;
        private static bool _showUnitPriceText = false;

        private static bool _defaultSortAscending = false;
        private static bool _enableDebugLogs = false;

        // === 公共属性（统一通过 Set<T> 触发保存与事件） ===
        [UIToggle]
        [Config("显示价格按钮")]
        public static bool ShowPriceButton
        {
            get => _showPriceButton;
            set => Set(ref _showPriceButton, value);
        }

        [UIToggle]
        [Config("显示重量按钮")]
        public static bool ShowWeightButton
        {
            get => _showWeightButton;
            set => Set(ref _showWeightButton, value);
        }

        [UIToggle]
        [Config("显示价重比按钮")]
        public static bool ShowRatioButton
        {
            get => _showRatioButton;
            set => Set(ref _showRatioButton, value);
        }

        [UIToggle]
        [Config("显示稀有度按钮")]
        public static bool ShowQualityButton
        {
            get => _showQualityButton;
            set => Set(ref _showQualityButton, value);
        }

        [UIToggle]
        [Config("显示单价按钮")]
        public static bool ShowUnitPriceButton
        {
            get => _showUnitPriceButton;
            set => Set(ref _showUnitPriceButton, value);
        }
        
        [UIToggle]
        [Config("显示ID按钮")]
        public static bool ShowIDButton
        {
            get => _showIDButton;
            set => Set(ref _showIDButton, value);
        }

        [UIToggle]
        [Config("显示价格信息")]
        public static bool ShowPriceText
        {
            get => _showPriceText;
            set => Set(ref _showPriceText, value);
        }

        [UIToggle]
        [Config("显示价重比信息")]
        public static bool ShowRatioText
        {
            get => _showRatioText;
            set => Set(ref _showRatioText, value);
        }

        [UIToggle]
        [Config("显示单价信息")]
        public static bool ShowUnitPriceText
        {
            get => _showUnitPriceText;
            set => Set(ref _showUnitPriceText, value);
        }

        [UIToggle]
        [Config("交换左右键排序方向")]
        public static bool DefaultSortAscending
        {
            get => _defaultSortAscending;
            set => Set(ref _defaultSortAscending, value);
        }
        public static bool EnableDebugLogs
        {
            get => _enableDebugLogs;
            set => Set(ref _enableDebugLogs, value);
        }

        // === 事件：配置变化通知 ===
        public static event Action? OnConfigChanged;

        // === 文件路径 ===
        private static readonly string ConfigDir =
            Path.Combine(Application.persistentDataPath, "Saves");

        private static readonly string ConfigName = $"{VersionInfo.Name}Config.json";
        private static readonly string ConfigPath = Path.Combine(ConfigDir, ConfigName);

        // === 统一更新接口 ===
        private static void Set<T>(ref T field, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (!Equals(field, newValue))
            {
                var oldValue = field;
                field = newValue;
                Save();
                OnConfigChanged?.Invoke();
                ModLogger.Info($"配置更新: {name} {oldValue} → {newValue}");
            }
        }

        // === 加载配置 ===
        public static void Load()
        {
            try
            {
                if (!Directory.Exists(ConfigDir))
                {
                    Directory.CreateDirectory(ConfigDir);
                    ModLogger.Warn($"配置目录不存在，已创建：{ConfigDir}");
                }

                if (!File.Exists(ConfigPath))
                {
                    ModLogger.Warn("配置文件不存在，创建默认配置...");
                    Save();
                    return;
                }

                string json = File.ReadAllText(ConfigPath);
                var cfg = JsonUtility.FromJson<TempConfig>(json);

                if (cfg == null)
                {
                    ModLogger.Error("配置文件内容不合法，已恢复默认配置。");
                    File.Delete(ConfigPath);
                    Save();
                    return;
                }

                _showPriceButton = cfg.ShowPriceButton;
                _showWeightButton = cfg.ShowWeightButton;
                _showRatioButton = cfg.ShowRatioButton;
                _showQualityButton = cfg.ShowQualityButton;
                _showUnitPriceButton = cfg.ShowUnitPriceButton;
                _showIDButton = cfg.ShowIDButton;

                _showPriceText = cfg.ShowPriceText;
                _showRatioText = cfg.ShowRatioText;
                _showUnitPriceText = cfg.ShowUnitPriceText;

                _defaultSortAscending = cfg.DefaultSortAscending;
                _enableDebugLogs = cfg.EnableDebugLogs;

                ModLogger.Info("配置加载完成。");
                OnConfigChanged?.Invoke();
            }
            catch (Exception ex)
            {
                ModLogger.Error("加载配置时发生异常，恢复默认配置。", ex);
                Save();
            }
        }

        // === 保存配置 ===
        public static void Save()
        {
            try
            {
                var cfg = new TempConfig
                {
                    ShowPriceButton = _showPriceButton,
                    ShowWeightButton = _showWeightButton,
                    ShowRatioButton = _showRatioButton,
                    ShowQualityButton = _showQualityButton,
                    ShowUnitPriceButton = _showUnitPriceButton,
                    ShowIDButton = _showIDButton,

                    ShowPriceText = _showPriceText,
                    ShowRatioText = _showRatioText,
                    ShowUnitPriceText = _showUnitPriceText,

                    DefaultSortAscending = _defaultSortAscending,
                    EnableDebugLogs = _enableDebugLogs
                };

                string json = JsonUtility.ToJson(cfg, true);
                File.WriteAllText(ConfigPath, json);

                ModLogger.Info($"配置已保存: {ConfigPath}");
            }
            catch (Exception ex)
            {
                ModLogger.Error("保存配置时发生异常。", ex);
            }
        }

        public static void InvokeConfigChanged()
        {
            OnConfigChanged?.Invoke();
        }

        // === 内部序列化类 ===
        [Serializable]
        private class TempConfig
        {
            public bool ShowPriceButton;
            public bool ShowWeightButton;
            public bool ShowRatioButton;
            public bool ShowQualityButton;
            public bool ShowUnitPriceButton;
            public bool ShowIDButton;
            public bool ShowPriceText;
            public bool ShowRatioText;
            public bool ShowUnitPriceText;
            public bool DefaultSortAscending;
            public bool EnableDebugLogs;
        }
    }
}
