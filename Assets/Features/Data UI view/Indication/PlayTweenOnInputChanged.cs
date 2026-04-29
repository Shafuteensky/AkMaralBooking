using DG.Tweening;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Проигрывает DOTweenAnimation при изменении значения TMP_InputField
    /// </summary>
    [RequireComponent(typeof(DOTweenAnimation))]
    public sealed class PlayTweenOnInputChanged : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private DOTweenAnimation tweenAnimation;

        private void Awake()
        {
            tweenAnimation = GetComponent<DOTweenAnimation>();
        }

        private void OnEnable()
        {
            if (inputField != null)
                inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            if (inputField != null)
                inputField.onValueChanged.RemoveListener(OnValueChanged);
        }

        /// <summary>
        /// Вызывается при изменении текста в поле
        /// </summary>
        private void OnValueChanged(string _)
        {
            if (tweenAnimation == null) return;

            tweenAnimation.DORestart();
        }
    }
}