using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для закрытия окна интерфейса
    /// </summary>
    public class ButtonCloseUIWindow : UIWindowControlButton
    {
        [SerializeField] protected bool needToOpenPrevious = true;
        
        /// <summary>
        /// Закрытие текущего окна в фокусе и открытие предыдущего
        /// </summary>
        public override void OnButtonClick()
        {
            UIWindowsController windowsController = UIWindowsController.Instance;
            
            if (!windowsController.LastOpenedWindow.PreviousWindow)
            {
                Debug.LogError($"Предыдущее окно не назначено для {windowsController.LastOpenedWindow.name}");
                return;
            }
            
            windowsController.CloseWindowById(parentUIWindow.Id.Id);
            if (needToOpenPrevious) windowsController.OpenPreviousWindow();
        }

#if UNITY_EDITOR
        [ContextMenu("Convert To ButtonCloseUIWindowAnimated")]
        protected void ConvertToAnimated()
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
