using System;
using Extensions.ScriptableValues;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Месячный календарь для выбора даты интервала аренды (с подсветкой дат в интервале выбора)
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
    
            dayView.SetFrame(false);

            bool hasArrivalDate = !arrivalDate.IsDefaultDate();
            bool hasDepartureDate = !departureDate.IsDefaultDate();

            if (!hasArrivalDate && !hasDepartureDate) return;

            if (hasArrivalDate && !hasDepartureDate)
            {
                dayView.SetFrame(date == arrivalDate.GetDate());
                return;
            }

            if (!hasArrivalDate && hasDepartureDate)
            {
                dayView.SetFrame(date == departureDate.GetDate());
                return;
            }

            bool dateInInterval = date >= arrivalDate.GetDate() && 
                                  date <= departureDate.GetDate();

            dayView.SetFrame(dateInInterval);
        }
    }
}