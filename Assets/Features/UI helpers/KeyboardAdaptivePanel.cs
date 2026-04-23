using DG.Tweening;
using Extensions.Log;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// Адаптирует UI-контейнер под экранную клавиатуру на мобильных устройствах
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class KeyboardAdaptivePanel : MonoBehaviour
    {
        [SerializeField] private float extraPadding = 24f;
        [SerializeField] private float animationDuration = 0.2f;

        private RectTransform rectTransform;
        private Canvas rootCanvas;

        private Vector2 defaultAnchoredPosition;
        private Vector2 defaultOffsetMin;
        private Vector2 defaultOffsetMax;

        private Tween moveTween;
        private float currentOffset;
        private float targetOffset;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas == null)
            {
                ServiceDebug.LogError($"Не найден Canvas в родителях у {name}, панель не будет адаптирована");
                enabled = false;
                return;
            }

            rootCanvas = parentCanvas.rootCanvas;
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
            float nextTargetOffset = GetKeyboardOffset();

            if (Mathf.Approximately(targetOffset, nextTargetOffset))
            {
                return;
            }

            targetOffset = nextTargetOffset;
            AnimateToOffset(targetOffset);
        }

        private void OnDisable()
        {
            KillTween();
            ApplyOffsetImmediate(0f);
            currentOffset = 0f;
            targetOffset = 0f;
        }

        private void OnDestroy()
        {
            KillTween();
        }

        private void CacheDefaultState()
        {
            defaultAnchoredPosition = rectTransform.anchoredPosition;
            defaultOffsetMin = rectTransform.offsetMin;
            defaultOffsetMax = rectTransform.offsetMax;
        }

        private bool IsVerticallyStretched()
        {
            return !Mathf.Approximately(rectTransform.anchorMin.y, rectTransform.anchorMax.y);
        }

        private float GetKeyboardOffset()
        {
            if (!TouchScreenKeyboard.visible)
            {
                return 0f;
            }

            Rect keyboardArea = TouchScreenKeyboard.area;
            if (keyboardArea.height <= 0f)
            {
                return 0f;
            }

            float canvasScaleFactor = rootCanvas.scaleFactor;
            if (canvasScaleFactor <= 0f)
            {
                canvasScaleFactor = 1f;
            }

            return keyboardArea.height / canvasScaleFactor + extraPadding;
        }

        private void AnimateToOffset(float offset)
        {
            KillTween();

            if (Mathf.Approximately(currentOffset, offset))
            {
                ApplyOffsetImmediate(offset);
                return;
            }

            moveTween = DOTween
                .To(() => currentOffset, ApplyOffsetImmediate, offset, animationDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true)
                .OnKill(() => moveTween = null);
        }

        private void ApplyOffsetImmediate(float offset)
        {
            currentOffset = offset;

            if (IsVerticallyStretched())
            {
                Vector2 offsetMin = defaultOffsetMin;
                offsetMin.y += offset;
                rectTransform.offsetMin = offsetMin;
                rectTransform.offsetMax = defaultOffsetMax;
                return;
            }

            Vector2 anchoredPosition = defaultAnchoredPosition;
            anchoredPosition.y += offset;
            rectTransform.anchoredPosition = anchoredPosition;
        }

        private void KillTween()
        {
            if (moveTween != null && moveTween.IsActive())
            {
                moveTween.Kill();
            }
        }
    }
}