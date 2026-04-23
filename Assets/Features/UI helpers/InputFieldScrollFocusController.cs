using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extensions.Log;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    /// <summary>
    /// Автоматически прокручивает ScrollRect так, чтобы активный TMP_InputField
    /// оставался в видимой области над экранной клавиатурой.
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
        private Coroutine trackingCoroutine;
        private Tween scrollTween;
        private float lastTargetNormalizedPosition = -1f;
        private Coroutine refreshFocusStateCoroutine;

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

        private void OnEnable()
        {
            RefreshInputFields();
        }

        private void OnDisable()
        {
            UnsubscribeFromInputFields();

            if (refreshFocusStateCoroutine != null)
            {
                StopCoroutine(refreshFocusStateCoroutine);
                refreshFocusStateCoroutine = null;
            }

            StopTracking();
            activeInputField = null;
        }

        private void OnDestroy()
        {
            KillScrollTween();
        }

        /// <summary>
        /// Пересобирает список полей ввода внутри ScrollRect и заново подписывается на их события.
        /// Вызывать после динамического создания или удаления TMP_InputField.
        /// </summary>
        public void RefreshInputFields()
        {
            UnsubscribeFromInputFields();

            inputFields.Clear();
            scrollRect.content.GetComponentsInChildren(true, inputFields);

            foreach (var inputField in inputFields)
            {
                if (inputField == null)
                {
                    continue;
                }

                inputField.onSelect.AddListener(OnInputSelected);
                inputField.onDeselect.AddListener(OnInputDeselected);
                inputField.onEndEdit.AddListener(OnInputEndEdit);
            }

            ResolveActiveInputField();

            if (activeInputField != null)
            {
                StartTracking();
            }
            else
            {
                StopTracking();
            }
        }

        private void OnInputSelected(string _)
        {
            ResolveActiveInputField();

            if (activeInputField == null)
            {
                return;
            }

            StartTracking();
        }

        private void OnInputDeselected(string _)
        {
            ScheduleRefreshFocusState();
        }

        private void OnInputEndEdit(string _)
        {
            ScheduleRefreshFocusState();
        }

        private IEnumerator RefreshFocusStateNextFrame()
        {
            yield return null;

            refreshFocusStateCoroutine = null;

            ResolveActiveInputField();

            if (activeInputField != null)
            {
                StartTracking();
                yield break;
            }

            if (!TouchScreenKeyboard.visible)
            {
                StopTracking();
            }
        }

        private void StartTracking()
        {
            if (trackingCoroutine != null)
            {
                return;
            }

            trackingCoroutine = StartCoroutine(TrackFocusedInputVisibility());
        }

        private void StopTracking()
        {
            if (trackingCoroutine != null)
            {
                StopCoroutine(trackingCoroutine);
                trackingCoroutine = null;
            }

            KillScrollTween();
            lastTargetNormalizedPosition = -1f;
        }

        private IEnumerator TrackFocusedInputVisibility()
        {
            yield return null;
            Canvas.ForceUpdateCanvases();
            EnsureActiveInputVisible();

            while (ShouldTrack())
            {
                ResolveActiveInputField();
                EnsureActiveInputVisible();
                yield return null;
            }
            
            KillScrollTween();
            trackingCoroutine = null;
            lastTargetNormalizedPosition = -1f;
        }

        private bool ShouldTrack()
        {
            return isActiveAndEnabled && (activeInputField != null || TouchScreenKeyboard.visible);
        }

        private void ResolveActiveInputField()
        {
            activeInputField = null;

            GameObject selectedObject = EventSystem.current != null
                ? EventSystem.current.currentSelectedGameObject
                : null;

            if (selectedObject != null)
            {
                TMP_InputField selectedInputField = selectedObject.GetComponentInParent<TMP_InputField>();

                if (selectedInputField != null && inputFields.Contains(selectedInputField))
                {
                    activeInputField = selectedInputField;
                    return;
                }
            }

            foreach (var inputField in inputFields)
            {
                if (inputField == null)
                {
                    continue;
                }

                if (inputField.isFocused)
                {
                    activeInputField = inputField;
                    return;
                }
            }
        }

        private void EnsureActiveInputVisible()
        {
            if (activeInputField == null
                || !scrollRect.vertical
                || scrollRect.content == null)
            {
                return;
            }

            float keyboardHeight = GetKeyboardHeightInScreenPixels();
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

        private float GetKeyboardHeightInScreenPixels()
        {
            if (!TouchScreenKeyboard.visible) return 0f;

            Rect keyboardArea = TouchScreenKeyboard.area;
            if (keyboardArea.height <= 0f) return 0f;

            return keyboardArea.height;
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
            foreach (var inputField in inputFields)
            {
                if (inputField == null) continue;

                inputField.onSelect.RemoveListener(OnInputSelected);
                inputField.onDeselect.RemoveListener(OnInputDeselected);
                inputField.onEndEdit.RemoveListener(OnInputEndEdit);
            }
        }
        
        private void ScheduleRefreshFocusState()
        {
            if (refreshFocusStateCoroutine != null) return;

            refreshFocusStateCoroutine = StartCoroutine(RefreshFocusStateNextFrame());
        }
    }
}