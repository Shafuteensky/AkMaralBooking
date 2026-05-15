using Extensions.Generics;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Действие кнопки отмены режима выбора диапазона дат
    /// </summary>
    public sealed class DeactivateDateRangeButtonAction : AbstractButtonAction
    {
        [Header("Зависимости"), Space]
        [SerializeField] private CalendarDayInteractionController controller;

        public override void OnButtonClickAction()
        {
            if (controller == null) return;
            controller.DeactivateDateRangeMode();
        }
    }
}
