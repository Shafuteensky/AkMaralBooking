using UnityEngine;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для открытия окна интерфейса
    /// </summary>
    public class ButtonActionOpenUIWindow : UIWindowControlButtonAction
    {
        /// <summary>
        /// Режим открытия
        /// </summary>
        public UIWindowOpenMode OpenMode => openMode;
        
        [Header("Параметры открытия"), Space]
        [SerializeField] protected UIWindowID UIWindowToOpen;
        [SerializeField] protected bool needToCloseThis = true;
        [Tooltip("Режим открытия: Forward — обычный переход вперёд, Pop — возврат к окну через обрезку хвоста истории")]
        [SerializeField] protected UIWindowOpenMode openMode = UIWindowOpenMode.Forward;

        public override void OnButtonClickAction() => windowsController.OpenWindow(
            UIWindowToOpen.Id, parentUIWindow, needToCloseThis, openMode);

        public override int GetPriority => 0;
    }
}