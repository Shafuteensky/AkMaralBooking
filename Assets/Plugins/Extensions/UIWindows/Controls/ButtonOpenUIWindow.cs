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
        [SerializeField] private bool closePrevious = false;
        [SerializeField] private UIWindowID closeWindowsUntil;

        public override void OnButtonClick() => OpenUIWindow();

        /// <summary>
        /// Открытие нового окна по нажатию на кнопку
        /// </summary>
        private void OpenUIWindow()
        {
            UIWindowsController windowsController = UIWindowsController.Instance;

            if (closeWindowsUntil)
            {
                if (!windowsController.CloseWindowsUntil(closeWindowsUntil.Id)) return;
                if (!UIWindowToOpen || UIWindowToOpen.Id == closeWindowsUntil.Id) return;

                windowsController.OpenWindowByID(UIWindowToOpen.Id, windowsController.LastOpenedWindow);
                return;
            }

            if (!UIWindowToOpen)
            {
                ServiceDebug.LogError($"Окно для открытия ({nameof(UIWindowToOpen)}) не назначено");
                return;
            }

            if (closePrevious && parentUIWindow.PreviousWindow)
                windowsController.CloseWindowById(parentUIWindow.PreviousWindow.Id);

            if (needToCloseThis)
                windowsController.CloseWindowById(parentUIWindow.Id.Id);

            windowsController.OpenWindowByID(UIWindowToOpen.Id, parentUIWindow);
        }
    }
}