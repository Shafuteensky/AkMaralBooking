using UnityEngine;

namespace Extensions.UIWindows
{
    using ID;
    
    /// <summary>
    /// Окно интерфейса
    /// </summary>
    public sealed class UIWindow : MonoBehaviour
    {
        public ID Id => id;
        public ID PreviousWindow => previousWindow;
        
        [SerializeField]
        private ID id = default;

        private ID previousWindow = default;
        
        public void SetPreviousWindow(ID window) => previousWindow = window;
    }
}