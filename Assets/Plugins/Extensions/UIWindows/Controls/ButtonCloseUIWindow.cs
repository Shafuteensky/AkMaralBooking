using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для закрытия окна интерфейса
    /// </summary>
    public sealed class ButtonCloseUIWindow : UIWindowControlButton
    {
        [SerializeField] private bool closeAll = false;
        [SerializeField] private bool needToOpenPrevious = true;

        public override void OnButtonClick() => CloseUIWindow();

        /// <summary>
        /// Закрытие текущего окна в фокусе и открытие предыдущего
        /// </summary>
        private void CloseUIWindow()
        {
            UIWindowsController windowsController = UIWindowsController.Instance;

            if (closeAll)
                windowsController.CloseAllWindows();
            else
                windowsController.CloseWindowById(parentUIWindow.Id.Id);

            if (!needToOpenPrevious) return;
            if (parentUIWindow.Type != UIWindowType.window) return;
            
            if (!windowsController.HasPreviousWindow(parentUIWindow))
            {
                Debug.LogError($"Предыдущее окно не назначено для {parentUIWindow.name}");
                return;
            }
            
            windowsController.OpenPreviousWindow(parentUIWindow);
        }

#if UNITY_EDITOR
        [ContextMenu("Convert To ButtonCloseUIWindowAnimated")]
        private void ConvertToAnimated()
        {
            GameObject go = gameObject;

            Undo.RecordObject(go, "Convert To ButtonCloseUIWindowAnimated");

            DestroyImmediate(this, true);
            go.AddComponent<ButtonCloseUIWindowAnimated>();

            EditorUtility.SetDirty(go);
        }
#endif
    }
}