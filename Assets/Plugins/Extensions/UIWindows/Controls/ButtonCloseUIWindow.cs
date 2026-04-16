using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Extensions.UIWindows
{
    /// <summary>
    /// Кнопка для закрытия окна интерфейса
    /// </summary>
    public class ButtonCloseUIWindow : UIWindowControlButton
    {
        [SerializeField] protected bool needToOpenPrevious = true;

        public override void OnButtonClick() => CloseUIWindow();

        /// <summary>
        /// Закрытие текущего окна в фокусе и открытие предыдущего
        /// </summary>
        protected void CloseUIWindow()
        {
            UIWindowsController windowsController = UIWindowsController.Instance;

            windowsController.CloseWindowById(parentUIWindow.Id.Id);

            if (!needToOpenPrevious) return;
            if (parentUIWindow.Type != UIWindowType.window) return;

            if (!windowsController.HasPreviousWindow(parentUIWindow))
            {
                Debug.LogError($"Предыдущее окно не назначено для {parentUIWindow.name}");
                return;
            }

            windowsController.OpenPreviousWindow(parentUIWindow);
        }

        /// <summary>
        /// Обновить параметры
        /// </summary>
        public void UpdateParams(bool needToOpenPrevious)
        {
            this.needToOpenPrevious = needToOpenPrevious;
        }
    }
}