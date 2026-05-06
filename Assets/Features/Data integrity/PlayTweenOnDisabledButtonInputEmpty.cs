using DG.Tweening;
using Extensions.Generics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Проигрывает DOTweenAnimation при попытке нажать отключенную кнопку с пустыми полями ввода
    /// </summary>
    public sealed class PlayTweenOnDisabledButtonInputEmpty : AbstractButton, IPointerClickHandler
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private DOTweenAnimation tweenAnimation;

        /// <summary>
        /// Вызывается при попытке нажатия
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (button.interactable || 
                !HasEmptyInputField() || 
                tweenAnimation == null) return;

            tweenAnimation.DORestart();
        }

        private bool HasEmptyInputField()
        {
            if (inputField == null) return false;;
            if (string.IsNullOrWhiteSpace(inputField.text)) return true;

            return false;
        }
    }
}