using UnityEngine;
using DG.Tweening;

namespace StarletBooking.Tweeners
{
    /// <summary>
    /// Воспроизводит анимацию при включении объекта (движение + fade)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class OpenScreenAnimation : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private bool moveEnabled = true;
        [SerializeField] private Vector3 fromOffset = new Vector3(-50f, 0f, 0f);
        [SerializeField] private float moveDuration = 0.25f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;

        [Header("Fade")]
        [SerializeField] private bool fadeEnabled = true;
        [SerializeField] private float fadeFrom = 0f;
        [SerializeField] private float fadeDuration = 0.25f;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;
        
        private RectTransform targetTransform;
        private CanvasGroup canvasGroup;

        private Sequence sequence;

        private void Awake()
        {
            targetTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable() => Play();

        private void OnDisable() => Kill();

        /// <summary>
        /// Проиграть анимацию
        /// </summary>
        public void Play()
        {
            Kill();

            if (targetTransform == null)
                targetTransform = transform as RectTransform;

            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            sequence = DOTween.Sequence();

            if (moveEnabled)
            {
                Vector3 startPos = targetTransform.anchoredPosition + (Vector2)fromOffset;
                targetTransform.anchoredPosition = startPos;

                sequence.Join(
                    targetTransform.DOAnchorPos(startPos - (Vector3)fromOffset, moveDuration)
                        .SetEase(moveEase)
                );
            }

            if (fadeEnabled)
            {
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = fadeFrom;

                    sequence.Join(
                        canvasGroup.DOFade(1f, fadeDuration)
                            .SetEase(fadeEase)
                    );
                }
            }

            sequence.Play();
        }

        /// <summary>
        /// Остановить анимацию
        /// </summary>
        public void Kill()
        {
            if (sequence != null && sequence.IsActive())
                sequence.Kill();
        }
    }
}