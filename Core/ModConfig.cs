using System;
using System.IO;
using UnityEngine;
using DuckSort.Utils; // 使用你的 ModLogger

namespace DuckSort.Core
{
    public static class ModConfig
    {
        // === 配置项 ===
        public static bool ShowPriceButton = true;
        public static bool ShowWeightButton = true;
        public static bool ShowRatioButton = true;

        public static bool ShowPriceText = true;
        public static bool ShowRatioText = true;

        public static bool DefaultSortAscending = false;  // false为降序、true为升序

        // === 路径定义 ===
        private static readonly string ConfigDir = Path.Combine(Application.persistentDataPath, "Saves");
        private static readonly string ConfigName = $"{VersionInfo.Name}Config.json";
        private static readonly string ConfigPath = Path.Combine(ConfigDir, ConfigName);

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
                ModLogger.Info($"读取配置文件: {ConfigPath}");

                var cfg = JsonUtility.FromJson<TempConfig>(json);
                if (cfg == null)
                {
                    ModLogger.Error("配置文件内容不合法，已恢复默认配置。");
                    Save();
                    return;
                }

                // 应用配置
                ShowPriceButton = cfg.ShowPriceButton;
                ShowWeightButton = cfg.ShowWeightButton;
                ShowRatioButton = cfg.ShowRatioButton;

                ShowPriceText = cfg.ShowPriceText;
                ShowRatioText = cfg.ShowRatioText;

                DefaultSortAscending = cfg.DefaultSortAscending;

                ModLogger.Info($"配置加载完成: " +
                    $"按钮(价格={ShowPriceButton}, 重量={ShowWeightButton}, 性价比={ShowRatioButton}) | " +
                    $"文本(价格={ShowPriceText}, 性价比={ShowRatioText}) | " +
                    $"升序={DefaultSortAscending}");
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
                    ShowPriceButton = ShowPriceButton,
                    ShowWeightButton = ShowWeightButton,
                    ShowRatioButton = ShowRatioButton,

                    ShowPriceText = ShowPriceText,
                    ShowRatioText = ShowRatioText,

                    DefaultSortAscending = DefaultSortAscending
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

        // === 数据结构（仅序列化使用） ===
        [Serializable]
        private class TempConfig
        {
            public bool ShowPriceButton;
            public bool ShowWeightButton;
            public bool ShowRatioButton;

            public bool ShowPriceText;
            public bool ShowRatioText;

            public bool DefaultSortAscending;
        }
    }
}
