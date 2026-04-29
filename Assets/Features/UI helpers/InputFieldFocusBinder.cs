using Extensions.Log;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Фокус на поле ввода после завершения обновления UI для мобильной клавиатуры
    /// </summary>
    [RequireComponent(typeof(InputFieldScrollFocusController))]
    public class InputFieldFocusBinder : MonoBehaviour
    {
        private KeyboardAdaptivePanel keyboardAdaptivePanel;
        private InputFieldScrollFocusController focusController;

        private void Awake()
        {
            focusController = GetComponent<InputFieldScrollFocusController>();
            keyboardAdaptivePanel = GetComponentInParent<KeyboardAdaptivePanel>();
        }

        private void OnEnable()
        {
            if (focusController == null || keyboardAdaptivePanel == null)
            {
                ServiceDebug.LogError("Не все компоненты инициализированы");
                return;
            }

            keyboardAdaptivePanel.onOffsetCompleted += focusController.RefreshFocusStateNextFrame;
        }

        private void OnDisable()
        {
            if (focusController == null || keyboardAdaptivePanel == null) return;

            keyboardAdaptivePanel.onOffsetCompleted -= focusController.RefreshFocusStateNextFrame;
        }
    }
}