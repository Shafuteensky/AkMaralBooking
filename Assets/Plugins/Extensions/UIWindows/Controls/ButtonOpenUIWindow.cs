using Extensions.Log;
using UnityEngine;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для открытия окна интерфейса
    /// </summary>
    public class ButtonOpenUIWindow : UIWindowControlButton
    {
        /// <summary>
        /// Режим открытия
        /// </summary>
        public UIWindowOpenMode OpenMode => openMode;
        
        [SerializeField] protected UIWindowID UIWindowToOpen;
        [SerializeField] protected bool needToCloseThis = true;
        [Tooltip("Режим открытия: Forward — обычный переход вперёд, Pop — возврат к окну через обрезку хвоста истории")]
        [SerializeField] protected UIWindowOpenMode openMode = UIWindowOpenMode.Forward;

        public override void OnButtonClick() => OpenUIWindow();

        /// <summary>
        /// Открытие нового окна по нажатию на кнопку
        /// </summary>
        protected void OpenUIWindow()
        {
            if (!UIWindowToOpen)
            {
                ServiceDebug.LogError($"Окно для открытия ({nameof(UIWindowToOpen)}) не назначено");
                return;
            }

            UIWindowsController windowsController = UIWindowsController.Instance;

            if (needToCloseThis && openMode == UIWindowOpenMode.Forward)
                windowsController.CloseWindowById(parentUIWindow.Id.Id);

            windowsController.OpenWindowByID(UIWindowToOpen.Id, parentUIWindow, true, openMode);
        }

        /// <summary>
        /// Обновить параметры
        /// </summary>
        public void UpdateParams(UIWindowID UIWindowToOpen, bool needToCloseThis, UIWindowOpenMode openMode)
        {
            this.UIWindowToOpen = UIWindowToOpen;
            this.needToCloseThis = needToCloseThis;
            this.openMode = openMode;
        }
    }
}