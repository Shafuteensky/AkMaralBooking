using System;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Кнопка назначения выборанной календарной даты в контейнер
    /// </summary>
    [RequireComponent(typeof(ReservationsMonthViewController))]
    public class CalendarDayDateSelectionButton : MonoBehaviour
    {
        [SerializeField] private DateValue dateValue;
        
        private ReservationsMonthViewController calendarController;

        private void Awake()
        {
            calendarController = GetComponent<ReservationsMonthViewController>();
        }

        private void OnEnable()
        {
            calendarController.onDayButtonClicked += UpdateDate;
        }

        private void OnDisable()
        {
            calendarController.onDayButtonClicked -= UpdateDate;
        }

        private void UpdateDate(DateTime date)
        {
            if (Logic.IsNull(dateValue)) return;
            dateValue.SetValue(date);
        }
    }
}