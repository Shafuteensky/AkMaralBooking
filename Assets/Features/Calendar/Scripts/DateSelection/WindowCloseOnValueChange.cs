using Extensions.ScriptableValues;
using Extensions.UIWindows;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Закрытие окна по готовности выбора даты
    /// </summary>
    public class WindowCloseOnValueChange : BaseScriptableValueListener<string, StringValue>
    {
        private UIWindow parentUIWindow;
        private UIWindowsController windowsController;
        
        private void Awake()
        {
            parentUIWindow = GetComponentInParent<UIWindow>();
            windowsController = UIWindowsController.Instance;
        }
        
        protected override void OnValueChanged(string value)
        {
            windowsController.CloseWindow(parentUIWindow);
        }
    }
}