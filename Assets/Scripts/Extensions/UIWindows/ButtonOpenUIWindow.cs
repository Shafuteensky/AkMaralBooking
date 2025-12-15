using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Extentions.UIWindows
{
    using ID;
    
    /// <summary>
    /// Кнопка для открытия окна интерфейса
    /// </summary>
    public class ButtonOpenUIWindow : AbstractOnClickButton
    {
        [SerializeField] 
        protected bool needToCloseFocused = true;
        
        [SerializeField] 
        protected ID UIWindowToOpen = default;
        
        protected override void OnButtonClick()
        {
            if (!UIWindowToOpen)
            {
                Debug.LogError("UIWindowsController.Instance.OpenWindowByID: UIWindowToOpen is null");
            }
            
            UIWindowsController windowsController = UIWindowsController.Instance;

            if (needToCloseFocused) 
                windowsController.CloseFocusedWindow();
            windowsController.OpenWindowByID(UIWindowToOpen.Id);
        }
    }
}