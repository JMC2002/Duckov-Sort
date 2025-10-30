using Duckov.UI;
using DuckSort.Core;
using DuckSort.UI;
using DuckSort.Utils;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DuckSort.Patches
{
    [HarmonyPatch(typeof(InventoryDisplay), "Setup")]
    public static class InventoryDisplay_Setup_Patch
    {
        public static readonly string PatchTag = "CustomSortButtons";

        static void Postfix(InventoryDisplay __instance)
        {
            try
            {
                if (SortButtons.Visibility.All(v => v == false))
                {
                    ModLogger.Info("所有排序按钮均被隐藏，跳过按钮创建");
                    return;
                }

                // 获取 sortButton 按钮
                var sortBtn = ReflectionHelper.GetFieldValue<Button>(__instance, "sortButton");
                if (sortBtn == null)
                {
                    ModLogger.Warn("sortButton 未找到，跳过按钮创建");
                    return;
                }

                // 如果原整理按钮被隐藏，则跳过，否则会在宠物背包上面创建按钮
                if (!sortBtn.gameObject.activeSelf || sortBtn.GetComponent<CanvasRenderer>().GetAlpha() == 0)
                {
                    ModLogger.Info("原整理按钮被隐藏，跳过创建新按钮");
                    return;
                }

                var container = __instance.transform.Find($"{VersionInfo.Name}_Container");
                if (container != null)
                {
                    // 如果已有按钮容器，先删除
                    ModLogger.Info("已有按钮容器，先删除");
                    UnityEngine.Object.Destroy(container.gameObject);
                }

                var sortRect = sortBtn.GetComponent<RectTransform>();
                var parentRect = sortRect.parent.GetComponent<RectTransform>();
                var grandParent = parentRect.parent.GetComponent<RectTransform>();

                // 创建新容器
                var newRowGO = new GameObject($"{VersionInfo.Name}_Container", typeof(RectTransform));
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

                var ValueLabel = L10n.GetLabel("按价值");
                var WeightLabel = L10n.GetLabel("按重量");
                var RatioLabel = L10n.GetLabel("按价重比");

                var inventory = __instance.Target;
                var sb = new SortButtons(newRowRT, sortBtn, inventory);
            }
            catch (Exception ex)
            {
                ModLogger.Error("创建按钮失败 ", ex);
            }
        }
    }
}
