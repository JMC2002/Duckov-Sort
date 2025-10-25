using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DuckSort.Utils;
using System.Linq;

namespace DuckSort.UI
{
    public class AddText
    {
        // 定义一个结构来描述每个显示字段
        private class TextEntry
        {
            public string? Label;
            public Func<Item, string>? ValueFunc;
            public bool Enabled;
            public TextMeshProUGUI? _text = null;
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
        }

        // 在这里注册所有可显示的字段
        private List<TextEntry>? entries
            = new ()
        {
            new TextEntry
            {
                Label = "价格",
                ValueFunc = item => $"${item.GetTotalRawValue() / 2:F0}",
                Enabled = true,
            },
            new TextEntry
            {
                Label = "重量",
                ValueFunc = item => $"{item.TotalWeight:F2}kg",
                Enabled = true,
            },
            new TextEntry
            {
                Label = "价重比",
                ValueFunc = item => {
                    var name = L10n.GetLabel("价重比");
                    if (item.TotalWeight < 0.001f)
                        return $"{name}: ∞";

                    float ratio = item.GetTotalRawValue() / item.TotalWeight;
                    return $"{name}: {(Math.Abs(ratio - MathF.Round(ratio)) < 0.001f ? ratio.ToString("F0") : ratio.ToString("F1"))}";
               },
                Enabled = true, // 默认关闭
            },
        };

// 启用/禁用事件绑定
public void Enable()
        {
            ModLogger.Info($"AddText.Enable() 调用时 TemplateTextUGUI = {GameplayDataSettings.UIStyle.TemplateTextUGUI}");
            ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;
            ModLogger.Info("AddText 已启用");
        }

        public void Disable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;

            // entries.Clear();
            // 销毁所有文本对象
            foreach (var entry in entries.Where(e => e.Enabled && e.Text != null))
            {
                ModLogger.Info($"销毁文本对象: {entry.Label}");
                UnityEngine.Object.Destroy(entry.Text.gameObject);
            }
            ModLogger.Info("AddText 已禁用");
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

                foreach (var entry in entries.Where(e => e.Enabled))
                {
                    if (entry.ValueFunc == null)
                    {
                        ModLogger.Error($"entry.ValueFunc 为 null，Label={entry.Label}");
                        continue;
                    }

                    var text = entry.Text;
                    if (text == null)
                    {
                        ModLogger.Error($"entry.Text 为 null，Label={entry.Label}");
                        continue;
                    }

                    text.gameObject.SetActive(true);
                    text.transform.SetParent(ui.LayoutParent);
                    text.transform.localScale = Vector3.one;
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
