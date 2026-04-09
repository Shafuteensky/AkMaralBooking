using Extensions.Generics;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Абстракция кнопки управления окнами <see cref="UIWindow"/>
    /// </summary>
    public abstract class UIWindowControlButton : AbstractButton
    {
        protected UIWindow parentUIWindow;
        
        protected override void Awake()
        {
            base.Awake();
            parentUIWindow = GetComponentInParent<UIWindow>();
        }
    }
}