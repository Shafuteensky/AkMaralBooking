using System;
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

            if (closePrevious) windowsController.CloseWindowById(parentUIWindow.PreviousWindow.Id);
            if (needToCloseThis) windowsController.CloseWindowById(parentUIWindow.Id.Id);
            
            windowsController.OpenWindowByID(UIWindowToOpen.Id);
        }
    }
}