using Duckov.UI;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using SodaCraft.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DuckSort
{
    public static class L10n
    {
        public static string GetLabel(string buttonType)
        {
            SystemLanguage language = LocalizationManager.CurrentLanguage;
            switch (language)
            {
                case SystemLanguage.ChineseSimplified:
                    return buttonType switch
                    {
                        "Value" => "按价值",
                        "Weight" => "按重量",
                        "Ratio" => "按价重比",
                        _ => buttonType
                    };

                case SystemLanguage.ChineseTraditional:
                    return buttonType switch
                    {
                        "Value" => "按價值",
                        "Weight" => "按重量",
                        "Ratio" => "按價重比",
                        _ => buttonType
                    };

                case SystemLanguage.Japanese:
                    return buttonType switch
                    {
                        "Value" => "価値",
                        "Weight" => "重量",
                        "Ratio" => "価値/重量順",
                        _ => buttonType
                    };

                case SystemLanguage.German:
                    return buttonType switch
                    {
                        "Value" => "Wert",
                        "Weight" => "Gewicht",
                        "Ratio" => "W/G",
                        _ => buttonType
                    };

                case SystemLanguage.Russian:
                    return buttonType switch
                    {
                        "Value" => "Ценность",
                        "Weight" => "Вес",
                        "Ratio" => "Ц/В",
                        _ => buttonType
                    };

                case SystemLanguage.Spanish:
                    return buttonType switch
                    {
                        "Value" => "Valor",
                        "Weight" => "Peso",
                        "Ratio" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.Korean:
                    return buttonType switch
                    {
                        "Value" => "가치",
                        "Weight" => "무게",
                        "Ratio" => "가/무",
                        _ => buttonType
                    };

                case SystemLanguage.French:
                    return buttonType switch
                    {
                        "Value" => "Valeur",
                        "Weight" => "Poids",
                        "Ratio" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.Portuguese:
                    return buttonType switch
                    {
                        "Value" => "Valor",
                        "Weight" => "Peso",
                        "Ratio" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.English:
                default:
                    return buttonType switch
                    {
                        "Value" => "Value",
                        "Weight" => "Weight",
                        "Ratio" => "Value/Weight",
                        _ => buttonType
                    };
            }
        }
    }

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {


        TextMeshProUGUI _text = null;
        TextMeshProUGUI Text
        {
            get
            {
                if (_text == null)
                {
                    _text = Instantiate(GameplayDataSettings.UIStyle.TemplateTextUGUI);
                }
                return _text;
            }
        }


        private const string HarmonyId = "DuckSort.CustomSortButtons";
        private Harmony? _harmony;

        void Awake()
        {
            Debug.Log("[DuckSort] 模组已加载");
        }
        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;
            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Debug.Log("[DuckSort] Patch applied");
        }
        void OnDestroy()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;
            if (_text != null)
                Destroy(_text);
        }

        void OnDisable()
        {
            _harmony?.UnpatchAll(HarmonyId);
            Debug.Log("[DuckSort] Patch removed");
        }

        private void OnSetupMeta(ItemHoveringUI uI, ItemMetaData data)
        {
            Text.gameObject.SetActive(false);
        }

        private void OnSetupItemHoveringUI(ItemHoveringUI uiInstance, Item item)
        {
            if (item == null)
            {
                Text.gameObject.SetActive(false);
                return;
            }

            Text.gameObject.SetActive(true);
            Text.transform.SetParent(uiInstance.LayoutParent);
            Text.transform.localScale = Vector3.one;
            Text.text = $"${item.GetTotalRawValue() / 2}";
            Text.fontSize = 20f;
        }
    }

    [HarmonyPatch(typeof(InventoryDisplay), "Setup")]
    public static class InventoryDisplay_Setup_Patch
    {
        static void Postfix(InventoryDisplay __instance)
        {
            try
            {
                // 获取 sortButton 按钮
                var sortBtnField = typeof(InventoryDisplay).GetField("sortButton", BindingFlags.NonPublic | BindingFlags.Instance);
                var sortBtn = sortBtnField?.GetValue(__instance) as Button;
                if (sortBtn == null)
                {
                    Debug.LogWarning("[DuckSort] sortButton 未找到，跳过按钮创建。");
                    return;
                }

                // 如果原整理按钮被隐藏，则跳过
                if (!sortBtn.gameObject.activeSelf || sortBtn.GetComponent<CanvasRenderer>().GetAlpha() == 0)
                {
                    Debug.Log("[DuckSort] 原整理按钮被隐藏，跳过创建新按钮。");
                    return;
                }

                // 防止重复创建
                if (__instance.transform.Find("DuckSort_Container") != null)
                    return;

                var sortRect = sortBtn.GetComponent<RectTransform>();
                var parentRect = sortRect.parent.GetComponent<RectTransform>();
                var grandParent = parentRect.parent.GetComponent<RectTransform>();

                // 创建新容器
                var newRowGO = new GameObject("DuckSort_Container", typeof(RectTransform));
                var newRowRT = newRowGO.GetComponent<RectTransform>();
                newRowRT.SetParent(grandParent, false);

                // 插入新行
                newRowRT.SetSiblingIndex(parentRect.GetSiblingIndex() + 1);

                // 设置布局属性
                var hGroup = newRowGO.AddComponent<HorizontalLayoutGroup>();
                hGroup.spacing = 8f;
                hGroup.childAlignment = TextAnchor.UpperRight;
                hGroup.childForceExpandWidth = false;
                hGroup.childForceExpandHeight = false;

                // 让新行与按钮行宽度对齐
                newRowRT.anchorMin = parentRect.anchorMin;
                newRowRT.anchorMax = parentRect.anchorMax;
                newRowRT.pivot = parentRect.pivot;
                newRowRT.sizeDelta = parentRect.sizeDelta;

                // 设置偏移量
                var height = sortRect.rect.height;
                newRowRT.anchoredPosition = parentRect.anchoredPosition - new Vector2(0, height + 8f);

                var ValueLabel = L10n.GetLabel("Value");
                var WeightLabel = L10n.GetLabel("Weight");
                var RatioLabel = L10n.GetLabel("Ratio");

                var inventory = __instance.Target;
                // 创建三个排序按钮
                CreateButton(newRowRT, sortBtn, ValueLabel, () =>
                {
                    Debug.Log("[DuckSort] 点击 按价值");
                    SortInventory(inventory, GetSortByValueComparison());
                });

                CreateButton(newRowRT, sortBtn, WeightLabel, () =>
                {
                    Debug.Log("[DuckSort] 点击 按重量");
                    SortInventory(inventory, GetSortByWeightComparison());
                });

                CreateButton(newRowRT, sortBtn, RatioLabel, () =>
                {
                    Debug.Log("[DuckSort] 点击 价重比");
                    SortInventory(inventory, GetSortByRatioComparison());
                });
                Debug.Log("[DuckSort] 成功在整理按钮正下方新增一行三个按钮。");
            }
            catch (Exception ex)
            {
                Debug.LogError("[DuckSort] 创建按钮失败: " + ex);
            }
        }

        static void CreateButton(RectTransform containerRT, Button templateButton, string label, UnityEngine.Events.UnityAction onClick)
        {
            var newGO = UnityEngine.Object.Instantiate(templateButton.gameObject, containerRT);
            newGO.name = "DuckSortBtn_" + label;

            TMP_Text tmp = newGO.GetComponentInChildren<TMP_Text>(true);
            if (tmp != null)
                tmp.text = label;
            else
            {
                var legacy = newGO.GetComponentInChildren<UnityEngine.UI.Text>(true);
                if (legacy != null)
                    legacy.text = label;
            }

            var btn = newGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(onClick);

            newGO.SetActive(true);
        }

        public static void SortInventory(Inventory inventory, Comparison<Item> comparison)
        {
            // 判断是否正在加载，防止重复操作
            if (inventory.Loading)
            {
                return;
            }

            inventory.Loading = true;

            // 获取私有的 content 字段
            var contentField = typeof(Inventory).GetField("content", BindingFlags.NonPublic | BindingFlags.Instance);
            var content = (List<Item>)contentField?.GetValue(inventory);

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
                // 使用反射调用 TryMerge（假设它是静态的）
                var tryMergeMethod = typeof(Inventory).GetMethod("TryMerge", BindingFlags.NonPublic | BindingFlags.Static);
                List<Item>? mergedItems = null;
                tryMergeMethod?.Invoke(null, new object[] { group, mergedItems });

                // 如果合并成功，添加到 sortedItems
                sortedItems.AddRange(mergedItems ?? group.ToList());
            }

            // 根据 Comparison 排序
            sortedItems.Sort(comparison);
            foreach (var item in sortedItems)
            {
                inventory.AddItem(item);
            }

            inventory.Loading = false;
            // 通过反射获取事件的委托，并手动调用
            var fieldInfo = typeof(Inventory).GetField("onInventorySorted", BindingFlags.NonPublic | BindingFlags.Instance);
            var eventDelegate = fieldInfo.GetValue(inventory) as Action<Inventory>;
            eventDelegate?.Invoke(inventory);
        }

        private static Comparison<Item> GetSortByValueComparison()
        {
            return (a, b) => b.GetTotalRawValue().CompareTo(a.GetTotalRawValue());
        }

        private static Comparison<Item> GetSortByWeightComparison()
        {
            return (a, b) => b.TotalWeight.CompareTo(a.TotalWeight);
        }

        private static Comparison<Item> GetSortByRatioComparison()
        {
            return (a, b) =>
            {
                float ratioA = a.GetTotalRawValue() / Math.Max(a.TotalWeight, 0.001f);
                float ratioB = b.GetTotalRawValue() / Math.Max(b.TotalWeight, 0.001f);
                return ratioB.CompareTo(ratioA);
            };
        }
    }
}
