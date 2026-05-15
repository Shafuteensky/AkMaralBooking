using Extensions.UIWindows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Расширение InputActionOpenUIWindow: в режиме выбора диапазона дат
    /// Cancel деактивирует режим, иначе — стандартное поведение.
    /// </summary>
    public sealed class InputActionOpenUIWindowWithModeCheck : InputActionOpenUIWindow
    {
        [Header("Режим выбора дат"), Space]
        [SerializeField] private CalendarDayInteractionController controller;

        protected override void OnInputPerformed(InputAction.CallbackContext context)
        {
            if (controller != null && controller.IsDateRangeActive)
            {
                controller.DeactivateDateRangeMode();
                return;
            }

            base.OnInputPerformed(context);
        }
    }
}
