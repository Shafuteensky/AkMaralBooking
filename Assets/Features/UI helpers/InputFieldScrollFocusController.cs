using System.Collections.Generic;
using DG.Tweening;
using Extensions.Coroutines;
using Extensions.Log;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StarletBooking.UI
{
    /// <summary>
    /// Автоматически прокручивает ScrollRect так, чтобы активный TMP_InputField
    /// оставался в видимой области над экранной клавиатурой
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public sealed class InputFieldScrollFocusController : MonoBehaviour
    {
        [SerializeField] private float bottomPadding = 24f;
        [SerializeField] private float scrollDuration = 0.2f;

        private readonly List<TMP_InputField> inputFields = new();
        private readonly Vector3[] worldCorners = new Vector3[4];

        private ScrollRect scrollRect;
        private RectTransform viewport;
        private Canvas rootCanvas;

        private TMP_InputField activeInputField;
        private Tween scrollTween;
        private float lastTargetNormalizedPosition = -1f;
        private bool refreshFocusStateScheduled;
        private bool trackingScheduled;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            viewport = scrollRect.viewport != null
                ? scrollRect.viewport
                : scrollRect.GetComponent<RectTransform>();
            rootCanvas = GetComponentInParent<Canvas>();

            if (viewport == null)
            {
                ServiceDebug.LogError($"Viewport не найден у {name}");
                enabled = false;
                return;
            }

            if (rootCanvas == null)
            {
                ServiceDebug.LogError($"Canvas не найден у {name}");
                enabled = false;
                return;
            }

            if (scrollRect.content == null)
            {
                ServiceDebug.LogError($"Content не найден у {name}");
                enabled = false;
                return;
            }
        }

        private void OnEnable() => RefreshInputFields();

        private void OnDisable()
        {
            UnsubscribeFromInputFields();

            refreshFocusStateScheduled = false;

            StopTracking();
            SetActiveInputField(null);
        }

        private void OnDestroy() => KillScrollTween();

        /// <summary>
        /// Пересобирает список полей ввода внутри ScrollRect и заново подписывается на их события
        /// Вызывать после динамического создания или удаления TMP_InputField
        /// </summary>
        public void RefreshInputFields()
        {
            UnsubscribeFromInputFields();

            inputFields.Clear();
            scrollRect.content.GetComponentsInChildren(true, inputFields);

            foreach (TMP_InputField inputField in inputFields)
            {
                if (inputField == null) continue;

                inputField.onSelect.AddListener(OnInputUpdate);
                inputField.onDeselect.AddListener(OnInputUpdate);
                inputField.onEndEdit.AddListener(OnInputUpdate);
            }

            ResolveActiveInputField();

            if (activeInputField != null)
                RefreshFocusStateNextFrame();
            else
                StopTracking();
        }

        /// <summary>
        /// Обновить фокус
        /// </summary>
        public void RefreshFocusStateNextFrame()
        {
            if (refreshFocusStateScheduled) return;

            refreshFocusStateScheduled = true;
            CoroutineDelay.Run(this, RefreshFocusState);
        }

        private void OnInputUpdate(string _) => RefreshFocusStateNextFrame();

        private void RefreshFocusState()
        {
            refreshFocusStateScheduled = false;

            Canvas.ForceUpdateCanvases();
            ResolveActiveInputField();

            if (activeInputField != null)
            {
                StartTracking();
                EnsureActiveInputVisible();
                return;
            }

            if (MobileScreenHelper.GetKeyboardHeightInScreenPixels() <= 0f) StopTracking();
        }

        private void StartTracking()
        {
            if (trackingScheduled) return;

            trackingScheduled = true;
            CoroutineDelay.Run(this, TrackFocusedInputVisibility);
        }

        private void StopTracking()
        {
            trackingScheduled = false;
            KillScrollTween();
            lastTargetNormalizedPosition = -1f;
        }

        private void TrackFocusedInputVisibility()
        {
            if (!trackingScheduled) return;

            Canvas.ForceUpdateCanvases();
            ResolveActiveInputField();
            EnsureActiveInputVisible();

            if (!ShouldTrack())
            {
                StopTracking();
                return;
            }

            CoroutineDelay.Run(this, TrackFocusedInputVisibility);
        }

        private bool ShouldTrack() => isActiveAndEnabled && (activeInputField != null || MobileScreenHelper.GetKeyboardHeightInScreenPixels() > 0f);

        private void ResolveActiveInputField()
        {
            GameObject selectedObject = EventSystem.current != null
                ? EventSystem.current.currentSelectedGameObject
                : null;

            if (selectedObject != null)
            {
                TMP_InputField selectedInputField = selectedObject.GetComponentInParent<TMP_InputField>();

                if (selectedInputField != null && inputFields.Contains(selectedInputField))
                {
                    SetActiveInputField(selectedInputField);
                    return;
                }
            }

            foreach (TMP_InputField inputField in inputFields)
            {
                if (inputField == null) continue;

                if (inputField.isFocused)
                {
                    SetActiveInputField(inputField);
                    return;
                }
            }

            SetActiveInputField(null);
        }

        private void SetActiveInputField(TMP_InputField inputField)
        {
            if (activeInputField == inputField) return;

            activeInputField = inputField;
            lastTargetNormalizedPosition = -1f;
            KillScrollTween();
        }

        private void EnsureActiveInputVisible()
        {
            if (activeInputField == null
                || !scrollRect.vertical
                || scrollRect.content == null)
            {
                return;
            }

            float keyboardHeight = MobileScreenHelper.GetKeyboardHeightInScreenPixels();
            if (keyboardHeight <= 0f) return;

            RectTransform inputRectTransform = activeInputField.GetComponent<RectTransform>();
            if (inputRectTransform == null) return;

            float canvasScaleFactor = rootCanvas.scaleFactor;
            if (canvasScaleFactor <= 0f) canvasScaleFactor = 1f;

            float visibleBottomScreenY = Mathf.Max(
                GetBottomScreenY(viewport),
                keyboardHeight + bottomPadding * canvasScaleFactor);

            float inputBottomScreenY = GetBottomScreenY(inputRectTransform);
            if (inputBottomScreenY >= visibleBottomScreenY) return;

            float hiddenContentHeight = scrollRect.content.rect.height - viewport.rect.height;
            if (hiddenContentHeight <= 0f) return;

            float hiddenPartInCanvasUnits = (visibleBottomScreenY - inputBottomScreenY) / canvasScaleFactor;

            float targetNormalizedPosition = Mathf.Clamp01(
                scrollRect.verticalNormalizedPosition - hiddenPartInCanvasUnits / hiddenContentHeight);

            if (Mathf.Abs(targetNormalizedPosition - lastTargetNormalizedPosition) < 0.001f) return;

            lastTargetNormalizedPosition = targetNormalizedPosition;
            ScrollTo(targetNormalizedPosition);
        }

        private float GetBottomScreenY(RectTransform target)
        {
            target.GetWorldCorners(worldCorners);

            float bottom = worldCorners[0].y;

            for (int i = 1; i < worldCorners.Length; i++)
            {
                if (worldCorners[i].y < bottom) bottom = worldCorners[i].y;
            }

            return bottom;
        }

        private void ScrollTo(float targetNormalizedPosition)
        {
            KillScrollTween();
            scrollRect.StopMovement();

            if (Mathf.Abs(scrollRect.verticalNormalizedPosition - targetNormalizedPosition) < 0.001f)
            {
                scrollRect.verticalNormalizedPosition = targetNormalizedPosition;
                return;
            }

            scrollTween = DOTween
                .To(
                    () => scrollRect.verticalNormalizedPosition,
                    value => scrollRect.verticalNormalizedPosition = value,
                    targetNormalizedPosition,
                    scrollDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true)
                .OnKill(() => scrollTween = null);
        }

        private void KillScrollTween()
        {
            if (scrollTween != null && scrollTween.IsActive()) scrollTween.Kill();
        }

        private void UnsubscribeFromInputFields()
        {
            foreach (TMP_InputField inputField in inputFields)
            {
                if (inputField == null) continue;

                inputField.onSelect.RemoveListener(OnInputUpdate);
                inputField.onDeselect.RemoveListener(OnInputUpdate);
                inputField.onEndEdit.RemoveListener(OnInputUpdate);
            }
        }
    }
}