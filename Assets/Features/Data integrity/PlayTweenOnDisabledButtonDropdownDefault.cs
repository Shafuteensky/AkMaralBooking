using DG.Tweening;
using Extensions.Generics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Проигрывает DOTweenAnimation при попытке нажать отключенную кнопку с невыбранными выпадающими списками
    /// </summary>
    public sealed class PlayTweenOnDisabledButtonDropdownDefault : AbstractButton, IPointerClickHandler
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private DOTweenAnimation tweenAnimation;

        /// <summary>
        /// Вызывается при попытке нажатия
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (button.interactable) return;
            if (!HasDefaultDropdown()) return;
            if (tweenAnimation == null) return;

            tweenAnimation.DORestart();
        }

        private bool HasDefaultDropdown()
        {
            if (dropdown == null) return false;
            if (dropdown.value == 0) return true;

            return false;
        }
    }
}