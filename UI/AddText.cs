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
            public TextMeshProUGUI Text => _text ??= UnityEngine.Object.Instantiate(
                                                            GameplayDataSettings.UIStyle.TemplateTextUGUI);
        }

        // 在这里注册所有可显示的字段
        private readonly List<TextEntry> entries = new()
        {
            new TextEntry
            {
                Label = "价格",
                ValueFunc = item => $"${item.GetTotalRawValue():F0}",
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
                Label = "性价比",
                ValueFunc = item =>
                {
                    float ratio = item.GetTotalRawValue() / Math.Max(item.TotalWeight, 0.001f);
                    return $"性价比: {ratio:F1}";
                },
                Enabled = true, // 默认关闭
            },
        };

        // 启用/禁用事件绑定
        public void Enable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;
            ModLogger.Info("AddText 已启用");
        }

        public void Disable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;
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
            foreach (var entry in entries.Where(e => e.Enabled))
            {
                entry.Text.gameObject.SetActive(false);
            }
        }

        private void OnSetupItemHoveringUI(ItemHoveringUI ui, Item item)
        {
            if (item == null)
            {
                foreach (var entry in entries.Where(e => e.Enabled))
                {
                    entry.Text.gameObject.SetActive(false);
                }
                ModLogger.Warn("Item 为 null，未显示任何文本");
                return;
            }

            foreach (var entry in entries.Where(e => e.Enabled))
            {
                entry.Text.gameObject.SetActive(true);
                entry.Text.transform.SetParent(ui.LayoutParent);
                entry.Text.transform.localScale = Vector3.one;
                if (entry.ValueFunc == null)
                {
                    ModLogger.Error($"TextEntry 的 ValueFunc 为 null，标签: {entry.Label}");
                    continue;
                }
                entry.Text.text = entry.ValueFunc(item);
                entry.Text.fontSize = 20f;
            }
        }
    }
}
