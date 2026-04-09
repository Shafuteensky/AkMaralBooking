using Extensions.Log;
using UnityEngine;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для открытия окна интерфейса
    /// </summary>
    public class ButtonOpenUIWindow : UIWindowControlButton
    {
        [SerializeField] protected bool needToCloseThis = true;
        [SerializeField] protected UIWindowID UIWindowToOpen;
        
        /// <summary>
        /// Открытие нового окна по нажатию на кнопку
        /// </summary>
        public override void OnButtonClick()
        {
            if (!UIWindowToOpen)
            {
                ServiceDebug.LogError($"Окно для открытия ({nameof(UIWindowToOpen)}) не назначено");
                return;
            }
            
            UIWindowsController windowsController = UIWindowsController.Instance;

            if (needToCloseThis) windowsController.CloseWindowById(parentUIWindow.Id.Id);
            windowsController.OpenWindowByID(UIWindowToOpen.Id);
        }
    }
}