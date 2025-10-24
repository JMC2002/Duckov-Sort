using DuckSort.Core;
using DuckSort.Utils;
using ItemStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DuckSort.UI
{
    public class SortButtonEntry
    {
        public string OrientedLabel { get; }    // 源文本，显示的是本地化的
        public string Label;
        public string Tag => $"{Label} ({OrientedLabel})";

        public UnityEngine.Events.UnityAction OnClick;
        public RectTransform containerRT;
        public Button templateButton;
        public bool activeSelf = true;          // 按钮是否可见，游戏的代码就叫这个
        public bool orderBy = false;            // 用于切换升序降序，默认为 false（降序）

        public SortButtonEntry(RectTransform containerRT, Button templateButton, Inventory inventory, string OrientedLabel, Comparison<Item> comparison)
        {
            this.OrientedLabel  = OrientedLabel;
            this.containerRT    = containerRT;
            this.templateButton = templateButton;

            Label = L10n.GetLabel(OrientedLabel);
            OnClick = () =>
            {
                ModLogger.Info($"点击 {Tag})");
                SortInventory(inventory, comparison);
            };

            Create();
        }

        public void Create()
        {
            if (activeSelf == false)
            {
                ModLogger.Info($"按钮 {Tag} 被设置为不可见，跳过创建。");
                return;
            }
            var newGO = UnityEngine.Object.Instantiate(templateButton.gameObject, containerRT);
            newGO.name = $"{VersionInfo.Name}Btn_" + Label;

            TMP_Text tmp = newGO.GetComponentInChildren<TMP_Text>(true);
            if (tmp != null)
                tmp.text = Label;
            else
            {
                var legacy = newGO.GetComponentInChildren<UnityEngine.UI.Text>(true);
                if (legacy != null)
                    legacy.text = Label;
            }

            var btn = newGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();   
            btn.onClick.AddListener(OnClick);

            newGO.SetActive(true);

            ModLogger.Info($"按钮 {Tag} 创建成功。");
        }

        public void ToggleOrder()
        {
            orderBy = !orderBy;
            ModLogger.Info($"按钮 {Tag} 切换排序方式为 {(orderBy ? "升序" : "降序")}");
        }

        private void SortInventory(Inventory inventory, Comparison<Item> comparison)
        {
            // 判断是否正在加载，防止重复操作
            if (inventory.Loading)
            {
                return;
            }

            inventory.Loading = true;

            ModLogger.Info($"开始对库存按{Tag}进行排序，当前为{(orderBy ? "升序" : "降序")}排序");

            var content = ReflectionHelper.GetFieldValue<List<Item>>(inventory, "content");
            if (content == null)
            {
                ModLogger.Warn("无法获取 Inventory.content，排序失败");
                inventory.Loading = false;
                return;
            }

            // 先筛选掉被锁定的 Item，并将其加入 list
            List<Item> list = new List<Item>();
            for (int i = 0; i < content.Count; i++)
            {
                if (!inventory.IsIndexLocked(i))
                {
                    Item item = content[i];
                    if (item != null)
                    {
                        item.Detach();
                        list.Add(item);
                    }
                }
            }

            // 按 TypeID 分组，并进行 TryMerge 操作
            List<IGrouping<int, Item>> groupedItems = list.GroupBy(item => item.TypeID).ToList();

            var sortedItems = new List<Item>();

            foreach (var group in groupedItems)
            {
                // 使用反射调用 TryMerge
                var args = new object[] { group, null! };
                bool success = ReflectionHelper.CallStaticMethodWithOut(
                    typeof(Inventory),
                    "TryMerge",
                    args,
                    out List<Item>? mergedItems
                );

                // 如果合并成功，添加到 sortedItems
                sortedItems.AddRange(mergedItems ?? group.ToList());
            }

            // 根据 Comparison 排序
            sortedItems.Sort((a, b) =>
            {
                int result = comparison(a, b);                              // 先按原有规则排序
                return result != 0 ? result : a.TypeID.CompareTo(b.TypeID); // 若相等则按 TypeID 比较，保证相同的 Item 放一起
            });

            foreach (var item in sortedItems)
            {
                inventory.AddItem(item);
            }

            inventory.Loading = false;
            // 通过反射获取事件的委托，并手动调用
            var eventDelegate = ReflectionHelper.GetFieldValue<Action<Inventory>>(inventory, "onInventorySorted");
            eventDelegate?.Invoke(inventory);
        }
    }

}
