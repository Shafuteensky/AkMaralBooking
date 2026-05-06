using DG.Tweening;
using Extensions.ScriptableValues;
using EZCalendarWeeklyView;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Проигрывает анимацию перелистывания месячного календаря
    /// </summary>
    [RequireComponent(typeof(MonthViewController))]
    public sealed class MonthViewChangeAnimationPlayer : MonoBehaviour
    {
        private const float OFFSET_Y = 30f;
        private const float DURATION = 0.25f;

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private BoolValue animationState;
        
        private ReservationsMonthViewController monthViewController;
        private Vector3 defaultAnchoredPosition;
        private Tween tween;

        private void Awake()
        {
            monthViewController = GetComponent<ReservationsMonthViewController>();
            defaultAnchoredPosition = rectTransform.anchoredPosition3D;
        }

        private void OnEnable()
        {
            monthViewController.onPreviousMonthChanged += PlayPreviousMonthAnimation;
            monthViewController.onNextMonthChanged += PlayNextMonthAnimation;
        }

        private void OnDisable()
        {
            monthViewController.onPreviousMonthChanged -= PlayPreviousMonthAnimation;
            monthViewController.onNextMonthChanged -= PlayNextMonthAnimation;
        }

        private void PlayPreviousMonthAnimation() => Play(OFFSET_Y);

        private void PlayNextMonthAnimation() => Play(-OFFSET_Y);

        private void Play(float offsetY)
        {
            if (animationState != null && !animationState.Value) return;
            
            tween?.Kill();

            rectTransform.anchoredPosition3D = defaultAnchoredPosition + new Vector3(0f, offsetY, 0f);
            tween = rectTransform
                .DOAnchorPos3D(defaultAnchoredPosition, DURATION)
                .SetEase(Ease.OutQuad);
        }
    }
}