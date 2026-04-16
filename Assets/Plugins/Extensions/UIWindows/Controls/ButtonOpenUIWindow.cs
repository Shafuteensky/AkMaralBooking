using Extensions.Log;
using UnityEngine;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для открытия окна интерфейса
    /// </summary>
    public class ButtonOpenUIWindow : UIWindowControlButton
    {
        [SerializeField] protected UIWindowID UIWindowToOpen;
        [SerializeField] protected bool needToCloseThis = true;
        [SerializeField] private UIWindowOpenMode openMode = UIWindowOpenMode.Forward;

        public override void OnButtonClick() => OpenUIWindow();

        /// <summary>
        /// Открытие нового окна по нажатию на кнопку
        /// </summary>
        private void OpenUIWindow()
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
    }
}