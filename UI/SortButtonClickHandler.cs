using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuckSort.UI
{
    /// <summary>
    /// 用于区分左右键点击 SortButton 的辅助组件。
    /// </summary>
    public class SortButtonClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public Button TargetButton = null!;
        public UnityEngine.Events.UnityAction? LeftClickAction;
        public UnityEngine.Events.UnityAction? RightClickAction;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (TargetButton == null) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                LeftClickAction?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClickAction?.Invoke();
            }
        }
    }
}
