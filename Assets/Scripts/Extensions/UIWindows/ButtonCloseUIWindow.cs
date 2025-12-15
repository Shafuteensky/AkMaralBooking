using UnityEngine;

namespace Extentions.UIWindows
{
    public class ButtonCloseUIWindow : AbstractButton
    {
        public override void OnButtonClick()
        {
            UIWindowsController windowsController = UIWindowsController.Instance;

            if (!windowsController.FocusedWindow.PreviousWindow)
            {
                Debug.LogError($"PreviousWindow is not set to {windowsController.FocusedWindow.name}");
                return;
            }

            windowsController.CloseFocusedWindow();
            windowsController.OpenPreviousWindow();
        }
    }
}
