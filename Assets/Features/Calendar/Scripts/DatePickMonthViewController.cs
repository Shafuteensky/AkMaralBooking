using System;
using Extensions.ScriptableValues;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Месячный календарь для выбора даты интервала аренды
    /// </summary>
    public class DatePickMonthViewController : ReservationsMonthViewController
    {
        [Header("Выделение"), Space]
        [SerializeField] private DateValue arrivalDate;
        [SerializeField] private DateValue departureDate;

        protected override void UpdateDayButton(Button button, DateTime date, Color color)
        {
            base.UpdateDayButton(button, date, color);
            
            ReservationCalendarDayView dayView = button.GetComponent<ReservationCalendarDayView>();

            if (dayView == null) return;

            bool dateInInterval = date >= arrivalDate.GetDate(DateTime.MinValue) && 
                                  date <= departureDate.GetDate(DateTime.MaxValue);
            dayView.SetFrame(dateInInterval);
        }
    }
}