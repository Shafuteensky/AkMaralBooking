using TMPro;
using UnityEngine;

namespace Extensions.UI
{
    /// <summary>
    /// Прокручивает текст TMP InputField по горизонтали в режиме просмотра.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public sealed class TMPInputFieldHorizontalTextScroller : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private float scrollSpeed = 25f;
        [SerializeField] private float edgePause = 1f;
        [SerializeField] private float sidePadding = 8f;

        private RectTransform textRectTransform;
        private RectTransform viewportRectTransform;

        private float startX;
        private float minX;
        private float pauseTimer;
        private int direction = -1;

        private void Awake()
        {
            if (inputField == null)
            {
                inputField = GetComponent<TMP_InputField>();
            }

            textRectTransform = inputField.textComponent.rectTransform;
            viewportRectTransform = inputField.textViewport;

            if (viewportRectTransform == null)
            {
                viewportRectTransform = inputField.GetComponent<RectTransform>();
            }

            startX = textRectTransform.anchoredPosition.x;
        }

        private void OnEnable()
        {
            ResetScroll();
        }

        private void Update()
        {
            if (inputField.isFocused)
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
        /// Сбрасывает положение текста.
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
            float textWidth = inputField.textComponent.preferredWidth;
            float viewportWidth = viewportRectTransform.rect.width - sidePadding * 2f;

            return textWidth > viewportWidth;
        }

        private void UpdateScrollBounds()
        {
            float textWidth = inputField.textComponent.preferredWidth;
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