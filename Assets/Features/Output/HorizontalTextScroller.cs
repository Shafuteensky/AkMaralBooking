using Extensions.Generics;
using UnityEngine;

namespace Extensions.UI
{
    /// <summary>
    /// Прокручивание текста по горизонтали, если он не помещается в viewport
    /// </summary>
    public sealed class HorizontalTextScroller : AbstractText
    {
        [Range(10f, 50f)]
        [SerializeField] private float scrollSpeed = 25f;
        [Range(0f, 10f)]
        [SerializeField] private float edgePause = 1f;
        [Range(0f, 15f)]
        [SerializeField] private float sidePadding = 8f;

        private RectTransform viewportRectTransform;
        private RectTransform textRectTransform;

        private float startX;
        private float minX;
        private float pauseTimer;
        private int direction = -1;

        public bool IsBlocked { get; set; }

        protected override void Awake()
        {
            base.Awake();
            
            textRectTransform = text.rectTransform;
            viewportRectTransform = textRectTransform.parent as RectTransform;
            startX = textRectTransform.anchoredPosition.x;
        }

        private void OnEnable() => ResetScroll();

        private void Update()
        {
            if (IsBlocked)
            {
                ResetScroll();
                return;
            }

            if (!IsOverflowing())
            {
                ResetScroll();
                return;
            }

            UpdateScrollBounds();
            ScrollText();
        }

        /// <summary>
        /// Сброс положения текста
        /// </summary>
        public void ResetScroll()
        {
            pauseTimer = edgePause;
            direction = -1;

            Vector2 anchoredPosition = textRectTransform.anchoredPosition;
            anchoredPosition.x = startX;
            textRectTransform.anchoredPosition = anchoredPosition;
        }

        private bool IsOverflowing()
        {
            float textWidth = text.preferredWidth;
            float viewportWidth = viewportRectTransform.rect.width - sidePadding * 2f;

            return textWidth > viewportWidth;
        }

        private void UpdateScrollBounds()
        {
            float textWidth = text.preferredWidth;
            float viewportWidth = viewportRectTransform.rect.width - sidePadding * 2f;

            minX = startX - (textWidth - viewportWidth);
        }

        private void ScrollText()
        {
            if (pauseTimer > 0f)
            {
                pauseTimer -= Time.unscaledDeltaTime;
                return;
            }

            Vector2 anchoredPosition = textRectTransform.anchoredPosition;
            anchoredPosition.x += direction * scrollSpeed * Time.unscaledDeltaTime;

            if (anchoredPosition.x <= minX)
            {
                anchoredPosition.x = minX;
                direction = 1;
                pauseTimer = edgePause;
            }
            else if (anchoredPosition.x >= startX)
            {
                anchoredPosition.x = startX;
                direction = -1;
                pauseTimer = edgePause;
            }

            textRectTransform.anchoredPosition = anchoredPosition;
        }
    }
}