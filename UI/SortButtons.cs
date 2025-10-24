using DuckSort.Utils;
using ItemStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
namespace DuckSort.UI
{
    internal class SortButtons
    {
        private static readonly (string Label, Comparison<Item> Comparison)[] SortModes =
        {
            ("按价格", (a, b) => b.GetTotalRawValue().CompareTo(a.GetTotalRawValue())),
            ("按重量", (a, b) => b.TotalWeight.CompareTo(a.TotalWeight)),
            ("按价重比", (a, b) =>
                    {
                        float ratioA = a.GetTotalRawValue() / Math.Max(a.TotalWeight, 0.001f);
                        float ratioB = b.GetTotalRawValue() / Math.Max(b.TotalWeight, 0.001f);
                        return ratioB.CompareTo(ratioA);
                    })
        };

        public static bool[] Visibility = { true, true, true };
        public SortedDictionary<int, SortButtonEntry> buttonDict;

        public SortButtons(RectTransform containerRT, Button templateButton, Inventory inventory)
        {
            System.Diagnostics.Debug.Assert(Visibility.Length == SortModes.Length, "Visibility 数组长度与 SortModes 不匹配！");

            var cnt = SortModes.Length;
            buttonDict = new SortedDictionary<int, SortButtonEntry>(
                Enumerable.Range(0, cnt)
                    .Where(i => Visibility[i]) // 只保留可见项
                    .Select(i => new
                    {
                        Index = i,
                        Entry = new SortButtonEntry(
                            containerRT,
                            templateButton,
                            inventory,
                            SortModes[i].Label,
                            SortModes[i].Comparison
                        )
                    })
                    .ToDictionary(x => x.Index, x => x.Entry)
            );
            ModLogger.Info($"成功新增一行 {SortModes.Length} 个按钮。");
        }


        public void toggleVisibility(int index)
        {
            if (index < 0 || index >= SortModes.Length)
            {
                ModLogger.Error($"尝试设置按钮可见性时索引越界: {index}，有效范围为 0 到 {SortModes.Length - 1}");
                return;
            }
            Visibility[index] = !Visibility[index];
            ModLogger.Info($"按钮索引 {index} 的可见性已更改为 {Visibility[index]}");
        }
    }
}
