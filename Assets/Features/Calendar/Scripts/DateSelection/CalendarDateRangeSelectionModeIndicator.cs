using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Включает/выключает индикаторный GameObject при смене режима выбора диапазона дат
    /// </summary>
    public sealed class CalendarDateRangeSelectionModeIndicator : MonoBehaviour
    {
        [Header("Зависимости"), Space]
        [SerializeField] private CalendarDayInteractionController controller;
        [SerializeField] private GameObject indicatorObject;

        private void OnEnable()
        {
            controller.onDateRangeModeChanged += SetIndicatorActive;
            SetIndicatorActive(controller.IsDateRangeActive);
        }

        private void OnDisable() => controller.onDateRangeModeChanged -= SetIndicatorActive;

        private void SetIndicatorActive(bool active)
        {
            if (Logic.IsNull(indicatorObject)) return;
            indicatorObject.SetActive(active);
        }
    }
}
