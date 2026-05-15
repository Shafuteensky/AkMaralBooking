using DG.Tweening;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using UnityEngine;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Индикатор состояния сворачивания панели — проигрывает анимацию по событию BoolValue
    /// </summary>
    public sealed class PanelCollapseIndicator : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation collapseAnimation;
        [SerializeField] private BoolValue collapseState;

        private void OnEnable()
        {
            if (Logic.IsNull(collapseState)) return;
            collapseState.onValueChanged += OnCollapseStateChanged;
        }

        private void Start()
        {
            if (collapseState == null) return;
            ApplyImmediate(collapseState.Value);
        }

        private void OnDisable()
        {
            if (collapseState == null) return;
            collapseState.onValueChanged -= OnCollapseStateChanged;
        }

        private void OnCollapseStateChanged(bool collapsed)
        {
            if (collapseAnimation == null) return;

            if (collapsed) collapseAnimation.tween.Restart();
            else collapseAnimation.tween.PlayBackwards();
        }

        private void ApplyImmediate(bool collapsed)
        {
            if (collapseAnimation?.tween == null) return;

            if (collapsed) collapseAnimation.tween.Complete();
            else collapseAnimation.tween.Rewind();
        }
    }
}