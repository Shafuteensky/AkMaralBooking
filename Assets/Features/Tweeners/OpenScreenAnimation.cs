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
        [Header("Move"), Space]
        [SerializeField] private bool moveEnabled = true;
        [SerializeField] private Vector3 fromOffset = new Vector3(-50f, 0f, 0f);
        [SerializeField] private float moveDuration = 0.25f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;

        [Header("Fade"), Space]
        [SerializeField] private bool fadeEnabled = true;
        [SerializeField] private float fadeFrom = 0f;
        [SerializeField] private float fadeDuration = 0.25f;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;

        private RectTransform targetTransform;
        private CanvasGroup canvasGroup;
        private Sequence sequence;
        private Vector3 defaultAnchoredPosition;

        private void Awake()
        {
            targetTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            defaultAnchoredPosition = targetTransform.anchoredPosition3D;
        }

        private void OnEnable()
        {
            Play();
        }

        private void OnDisable()
        {
            Kill();
            RestoreDefaultState();
        }

        /// <summary>
        /// Проиграть анимацию
        /// </summary>
        public void Play()
        {
            Kill();
            RestoreClosedState();

            sequence = DOTween.Sequence();

            if (moveEnabled)
            {
                sequence.Join(
                    targetTransform.DOAnchorPos3D(defaultAnchoredPosition, moveDuration)
                        .SetEase(moveEase)
                );
            }

            if (fadeEnabled)
            {
                sequence.Join(
                    canvasGroup.DOFade(1f, fadeDuration)
                        .SetEase(fadeEase)
                );
            }

            sequence.Play();
        }

        /// <summary>
        /// Остановить анимацию
        /// </summary>
        public void Kill()
        {
            if (sequence != null && sequence.IsActive())
            {
                sequence.Kill();
                sequence = null;
            }
        }

        /// <summary>
        /// Восстановить стартовое скрытое состояние
        /// </summary>
        private void RestoreClosedState()
        {
            if (moveEnabled)
                targetTransform.anchoredPosition3D = defaultAnchoredPosition + fromOffset;
            else
                targetTransform.anchoredPosition3D = defaultAnchoredPosition;

            if (fadeEnabled)
                canvasGroup.alpha = fadeFrom;
            else
                canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// Восстановить нормальное состояние
        /// </summary>
        private void RestoreDefaultState()
        {
            targetTransform.anchoredPosition3D = defaultAnchoredPosition;
            canvasGroup.alpha = 1f;
        }
    }
}