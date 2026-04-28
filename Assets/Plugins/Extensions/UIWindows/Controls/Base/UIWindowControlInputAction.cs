using Extensions.Generics.Input;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Абстракция ввода управления окнами <see cref="UIWindow"/>
    /// </summary>
    public abstract class UIWindowControlInputAction : AbstractInputAction
    {
        protected UIWindow parentUIWindow;
        protected UIWindowsController windowsController;
        
        protected virtual void Awake()
        {
            parentUIWindow = GetComponentInParent<UIWindow>();
            windowsController = UIWindowsController.Instance;
        }
    }
}