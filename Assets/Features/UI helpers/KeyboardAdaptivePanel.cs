using System;
using DG.Tweening;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Адаптация full stretch панели под мобильную клавиатуру
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class KeyboardAdaptivePanel : MonoBehaviour
    {
        /// <summary>
        /// Событие окончания обновления положения панели
        /// </summary>
        public event Action onOffsetCompleted;
        
        [SerializeField] private float extraPadding = 24f;
        [SerializeField] private float animationDuration = 0.2f;

        private RectTransform rectTransform;

        private Vector2 defaultOffsetMin;
        private Vector2 defaultOffsetMax;

        private float currentOffset;
        private float targetOffset;

        private Tween tween;
        

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            CacheDefaultState();

            currentOffset = 0f;
            targetOffset = 0f;
        }

        private void OnEnable()
        {
            CacheDefaultState();
            ApplyOffsetImmediate(0f);
        }

        private void Update()
        {
            float nextOffset = GetKeyboardHeight();

            if (Mathf.Approximately(targetOffset, nextOffset)) return;

            targetOffset = nextOffset;
            AnimateTo(targetOffset);
        }

        private void OnDisable()
        {
            KillTween();
            ApplyOffsetImmediate(0f);

            currentOffset = 0f;
            targetOffset = 0f;
        }

        private void OnDestroy() => KillTween();

        /// <summary>
        /// Обновить дефолтное состояние панели
        /// </summary>
        public void RefreshDefaultPosition()
        {
            defaultOffsetMin = rectTransform.offsetMin;
            defaultOffsetMax = rectTransform.offsetMax;

            if (!Mathf.Approximately(currentOffset, 0f))
            {
                defaultOffsetMin.y -= currentOffset;
            }
        }

        private void CacheDefaultState()
        {
            defaultOffsetMin = rectTransform.offsetMin;
            defaultOffsetMax = rectTransform.offsetMax;
        }

        private float GetKeyboardHeight()
        {
            float keyboardHeight = MobileScreenHelper.GetKeyboardHeightInCanvasUnits();

            if (keyboardHeight <= 0f) return 0f;

            return keyboardHeight + extraPadding;
        }

        private void AnimateTo(float offset)
        {
            KillTween();

            if (Mathf.Approximately(currentOffset, offset))
            {
                ApplyOffsetImmediate(offset);
                return;
            }
            
            tween = DOTween
                .To(() => currentOffset, ApplyOffsetImmediate, offset, animationDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true)
                .OnComplete(() => onOffsetCompleted?.Invoke())
                .OnKill(() => tween = null);
        }

        private void ApplyOffsetImmediate(float offset)
        {
            currentOffset = offset;

            Vector2 offsetMin = defaultOffsetMin;
            offsetMin.y = defaultOffsetMin.y + offset;

            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = defaultOffsetMax;
        }

        private void KillTween()
        {
            if (tween != null && tween.IsActive()) tween.Kill();
        }
    }
}