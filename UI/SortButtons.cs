using DuckSort.Core;
using DuckSort.Utils;
using ItemStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DuckSort.UI
{
    internal class SortButtons
    {
        private static readonly (string Label, Comparison<Item> Comparison)[] SortModes =
        {
            ("按价值", (a, b) => b.GetTotalRawValue().CompareTo(a.GetTotalRawValue())),
            ("按重量", (a, b) => b.TotalWeight.CompareTo(a.TotalWeight)),
            ("按价重比", (a, b) =>
                    {
                        float ratioA = a.GetTotalRawValue() / Math.Max(a.TotalWeight, 0.001f);
                        float ratioB = b.GetTotalRawValue() / Math.Max(b.TotalWeight, 0.001f);
                        return ratioB.CompareTo(ratioA);
                    }),
            ("按稀有度", (a, b) => b.Quality.CompareTo(a.Quality)),
            ("按单价", (a, b) => {
                // 这里不要直接调用a.Value，这样不会计算嵌在里面的物品价值（比如配件、子弹、包包）
                Func<Item, float> f = (i) => i.GetTotalRawValue() / i.StackCount;
                return f(b).CompareTo(f(a));
            }),
            ("按ID", (a, b) => b.TypeID.CompareTo(a.TypeID)),
        };

        public static bool[] Visibility => new bool[]{ 
            ModConfig.ShowPriceButton,
            ModConfig.ShowWeightButton,
            ModConfig.ShowRatioButton,
            ModConfig.ShowQualityButton,
            ModConfig.ShowUnitPriceButton,
            ModConfig.ShowIDButton,
        };
        public SortedDictionary<int, SortButtonEntry> buttonDict;

        public SortButtons(RectTransform containerRT, Button templateButton, Inventory inventory)
        {
            System.Diagnostics.Debug.Assert(Visibility.Length == SortModes.Length, "Visibility 数组长度与 SortModes 不匹配！");

            var cnt = SortModes.Length;
            buttonDict = new SortedDictionary<int, SortButtonEntry>(
                Enumerable.Range(0, cnt)
                    .Select(i =>
                    {
                        if (!Visibility[i])
                        {
                            ModLogger.Info($"排序模式 {SortModes[i].Label} 已隐藏");
                            return (Index: i, Entry: (SortButtonEntry?)null);
                        }

                        var entry = new SortButtonEntry(
                            containerRT,
                            templateButton,
                            inventory,
                            SortModes[i].Label,
                            SortModes[i].Comparison
                        );

                        return (Index: i, Entry: entry);
                    })
                    .Where(x => x.Entry != null) // 去掉隐藏的项
                    .ToDictionary(x => x.Index, x => x.Entry!)
            );
            ModLogger.Info($"成功新增一行 {SortModes.Length} 个按钮。");
        }


        //public void ToggleVisibility(int index)
        //{
        //    if (index < 0 || index >= SortModes.Length)
        //    {
        //        ModLogger.Error($"尝试设置按钮可见性时索引越界: {index}，有效范围为 0 到 {SortModes.Length - 1}");
        //        return;
        //    }
        //    Visibility[index] = !Visibility[index];
        //    ModLogger.Info($"按钮索引 {index} 的可见性已更改为 {Visibility[index]}");
        //}
    }
}
