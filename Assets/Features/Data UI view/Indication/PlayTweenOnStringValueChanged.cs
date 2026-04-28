using DG.Tweening;
using Extensions.ScriptableValues;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Проигрывает DOTweenAnimation при изменении StringValue
    /// </summary>
    [RequireComponent(typeof(DOTweenAnimation))]
    public sealed class PlayTweenOnStringValueChanged : MonoBehaviour
    {
        [SerializeField] private StringValue stringValue;
        [SerializeField] private TMP_InputField inputField;

        private DOTweenAnimation tweenAnimation;

        private void Awake()
        {
            tweenAnimation = GetComponent<DOTweenAnimation>();
        }

        private void OnEnable()
        {
            if (stringValue == null) return;

            stringValue.onValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            if (stringValue == null) return;

            stringValue.onValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(string _)
        {
            if (tweenAnimation == null) return;

            if (inputField != null && inputField.isFocused)
                return;

            tweenAnimation.DORestart();
        }
    }
}