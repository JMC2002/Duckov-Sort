using DuckSort.Core;
using DuckSort.Patches;
using DuckSort.UI;
using DuckSort.Utils;

namespace DuckSort
{

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public AddText addText = new();

        private HarmonyHelper harmonyHelper = new("CustomSortButtons");

        void Awake()
        {
            ModConfig.Load();
            ModLogger.Info("模组已启用");
        }
        void OnEnable()
        {
            addText.Enable();
            harmonyHelper.OnEnable();
        }

        void OnDestroy()
        {
        }

        void OnDisable()
        {
            addText.Disable();
            harmonyHelper.OnDisable();

            ModConfig.Save();
            ModLogger.Info("Mod 已禁用，配置已保存");
        }
    }
}
