using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Вспомогательные методы для работы с параметрами экрана на мобильных устройствах
    /// </summary>
    public static class MobileScreenHelper
    {
        private const float ReferenceHeight = 853f;

        /// <summary>
        /// Получить высоту клавиатуры в screen pixels
        /// </summary>
        public static float GetKeyboardHeightInScreenPixels()
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

                        if (keyboardHeight <= 0)
                        {
                            return 0f;
                        }

                        return keyboardHeight;
                    }
                }
            }
            catch
            {
                return 0f;
            }

#elif UNITY_IOS
            if (!TouchScreenKeyboard.visible)
            {
                return 0f;
            }

            Rect keyboardArea = TouchScreenKeyboard.area;
            if (keyboardArea.height <= 0f)
            {
                return 0f;
            }

            return keyboardArea.height;

#else
            return 0f;
#endif
        }

        /// <summary>
        /// Перевести screen pixels в canvas units
        /// </summary>
        public static float ConvertScreenPixelsToCanvasUnits(float screenPixels)
        {
            if (screenPixels <= 0f)
            {
                return 0f;
            }

            float scaleFactor = Screen.height / ReferenceHeight;
            if (scaleFactor <= 0f)
            {
                scaleFactor = 1f;
            }

            return screenPixels / scaleFactor;
        }

        /// <summary>
        /// Получить высоту клавиатуры в canvas units
        /// </summary>
        public static float GetKeyboardHeightInCanvasUnits()
        {
            float keyboardHeightInScreenPixels = GetKeyboardHeightInScreenPixels();
            return ConvertScreenPixelsToCanvasUnits(keyboardHeightInScreenPixels);
        }
    }
}