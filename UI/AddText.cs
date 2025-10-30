using Duckov.UI;
using Duckov.Utilities;
using DuckSort.Core;
using DuckSort.Utils;
using ItemStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DuckSort.UI
{
    public class AddText
    {
        private bool isEnabled = false;

        // 定义一个结构来描述每个显示字段
        private class TextEntry
        {
            public string? Label;
            public Func<Item, string>? ValueFunc;

            private readonly Func<bool>? enabledGetter;
            public bool Enabled => enabledGetter?.Invoke() ?? false;

            public TextMeshProUGUI? _text = null;

            // 此处不能直接写成public TextMeshProUGUI Text => _text ??= UnityEngine.Object.Instantiate(GameplayDataSettings.UIStyle.TemplateTextUGUI);

            public TextMeshProUGUI Text
            {
                get
                {
                    if (_text == null)
                    {
                        _text = UnityEngine.Object.Instantiate(GameplayDataSettings.UIStyle.TemplateTextUGUI);
                    }
                    return _text;
                }
            }

            public TextEntry(string label, Func<Item, string> valueFunc, Func<bool> enabledGetter)
            {
                Label = label;
                ValueFunc = valueFunc;
                this.enabledGetter = enabledGetter;
            }
        }

        // 在这里注册所有可显示的字段
        private List<TextEntry> entries = new();

        public void Enable()
        {
            if (isEnabled)
                return;

            // 清理旧的条目（如果有的话）
            entries.Clear();

            entries.Add(new TextEntry(
                     "价格",
                     item => $"${item.GetTotalRawValue() / 2:F0}",
                     () => ModConfig.ShowPriceText
                 ));

            entries.Add(new TextEntry(
                     "重量",
                     item => $"{item.TotalWeight:F2}kg",
                     () => false // 默认关闭
                 ));

            entries.Add(new TextEntry(
                     "价重比",
                     item => {
                         var name = L10n.GetLabel("价重比");
                         if (item.TotalWeight < 0.001f)
                             return $"{name}: ∞";
            
                         float ratio = item.GetTotalRawValue() / 2 / item.TotalWeight;
                         // ModLogger.Info($"计算价重比: 总价值={item.GetTotalRawValue() / 2}, 总重量={item.TotalWeight}, 价重比={ratio}");
                         return $"{name}: {(Math.Abs(ratio - MathF.Round(ratio)) < 0.001f ? ratio.ToString("F0") : ratio.ToString("F1"))}";
                    },
                     () => ModConfig.ShowRatioText
                 ));
            
             ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
             ItemHoveringUI.onSetupMeta += OnSetupMeta;
            
             // 订阅配置变化事件
             ModConfig.OnConfigChanged += OnConfigChanged;
            
             isEnabled = true;
             ModLogger.Info("AddText 已启用");
        }

        public void Disable()
        {
            if (!isEnabled)
                return;

            ItemHoveringUI.onSetupItem -= OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;

            ModConfig.OnConfigChanged -= OnConfigChanged;

            // 销毁所有已创建的文本对象
            foreach (var entry in entries.Where(e => e.Text != null))
            {
                ModLogger.Info($"销毁文本对象: {entry.Label}");
                UnityEngine.Object.Destroy(entry.Text.gameObject);
            }

            ModLogger.Info("AddText 已禁用");
        }

        private void OnConfigChanged()
        {
            ModLogger.Info("AddText 配置已更新。");
        }


        private void OnSetupMeta(ItemHoveringUI ui, ItemMetaData data)
        {
            if (ui == null)
            {
                ModLogger.Warn("ui 为 null，未显示任何文本");
            }
            foreach (var entry in entries.Where(e => e.Enabled))
            {
                entry.Text.gameObject.SetActive(false);
            }
        }

        private void OnSetupItemHoveringUI(ItemHoveringUI ui, Item item)
        {
            try
            {
                if (ui == null)
                {
                    ModLogger.Error("ui 为 null！");
                    return;
                }

                if (ui.LayoutParent == null)
                {
                    ModLogger.Error("ui.LayoutParent 为 null！");
                    return;
                }

                if (item == null)
                {
                    ModLogger.Warn("item 为 null！");
                    foreach (var entry in entries.Where(e => e.Enabled))
                        entry.Text.gameObject.SetActive(false);
                    return;
                }

                foreach (var entry in entries)
                {
                    bool enabled = entry.Enabled;
                    entry.Text.gameObject.SetActive(enabled);
                    if (!enabled) continue;

                    var text = entry.Text;
                    text.transform.SetParent(ui.LayoutParent);
                    text.transform.localScale = Vector3.one;

                    if (entry.ValueFunc == null)
                    {
                        ModLogger.Warn($"ValueFunc 为 null，跳过 {entry.Label}");
                        continue;
                    }
                    text.text = entry.ValueFunc(item);
                    text.fontSize = 20f;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"OnSetupItemHoveringUI 抛出异常", ex);
            }
        }
    }
}
