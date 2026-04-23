using DG.Tweening;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// Адаптация full stretch панели под клавиатуру (Android + iOS)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class KeyboardAdaptivePanel : MonoBehaviour
    {
        [SerializeField] private float extraPadding = 24f;
        [SerializeField] private float animationDuration = 0.2f;

        private RectTransform rectTransform;

        private Vector2 defaultOffsetMin;
        private Vector2 defaultOffsetMax;

        private float currentOffset;
        private float targetOffset;

        private const float referenceHeight = 853f;
        
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

        private void CacheDefaultState()
        {
            defaultOffsetMin = rectTransform.offsetMin;
            defaultOffsetMax = rectTransform.offsetMax;
        }

        /// <summary>
        /// Получить высоту клавиатуры (Android / iOS)
        /// </summary>
        private float GetKeyboardHeight()
        {
#if UNITY_EDITOR
            return 0f;

#elif UNITY_ANDROID
            try
            {
                using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject unityPlayer = currentActivity.Get<AndroidJavaObject>("mUnityPlayer");
                    AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");

                    using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
                    {
                        view.Call("getWindowVisibleDisplayFrame", rect);

                        int screenHeight = Screen.height;
                        int rectHeight = rect.Call<int>("height");
                        int keyboardHeight = screenHeight - rectHeight;

                        float scaleFactor = screenHeight / referenceHeight;
                        float result = keyboardHeight / scaleFactor;

                        return result > 0 ? result + extraPadding : 0f;
                    }
                }
            }
            catch (System.Exception e)
            {
                return 0f;
            }

#elif UNITY_IOS
            int screenHeight = Screen.height;
            int keyboardHeight = (int)TouchScreenKeyboard.area.height;

            float scaleFactor = screenHeight / referenceHeight;
            float result = keyboardHeight / scaleFactor;

            return result > 0 ? result + extraPadding : 0f;

#else
            return 0f;
#endif
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