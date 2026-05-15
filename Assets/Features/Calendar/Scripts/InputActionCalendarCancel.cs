using Extensions.UIWindows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Действие или отмена режима ввода дат новой записи
    /// </summary>
    public sealed class InputActionCalendarCancel : InputActionOpenUIWindow
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
