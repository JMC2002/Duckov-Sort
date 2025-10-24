using Duckov.UI;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace DuckSort
{
    using Core;
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


        private string HarmonyId = $"{VersionInfo.Name}.CustomSortButtons";
        private Harmony? _harmony;

        void Awake()
        {
            ModLogger.Info("模组已加载");
        }
        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;
            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModLogger.Info("Harmony 补丁已加载");
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
            ModLogger.Info("Harmony 补丁已卸载");
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
}
