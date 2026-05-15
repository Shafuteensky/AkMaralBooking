using Extensions.Generics;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Действие зажатия кнопки дня для активации режима выбора диапазона дат.
    /// Не хранит дату — только сигнализирует контроллеру о завершённом зажатии.
    /// </summary>
    public sealed class HoldDayButtonActivateDateRangeMode : AbstractHoldButtonAction
    {
        [Header("Зависимости"), Space]
        [SerializeField] private CalendarDayInteractionController controller;

        public override void OnButtonClickAction()
        {
            if (controller == null) return;
            controller.SetPendingHold();
        }
    }
}
