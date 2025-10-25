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
            ModLogger.Info("模组已加载");
        }
        void OnEnable()
        {
            addText.Enable();
            harmonyHelper.OnEnable();
        }

        void OnDestroy()
        {
            addText.Disable();
        }

        void OnDisable()
        {
            harmonyHelper.OnDisable();
        }
    }
}
