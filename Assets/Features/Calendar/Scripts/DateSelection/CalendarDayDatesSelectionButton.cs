using System;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Кнопка назначения выборанной календарной даты в пустой контейнер
    /// </summary>
    [RequireComponent(typeof(ReservationsMonthViewController))]
    public class CalendarDayDatesSelectionButton : MonoBehaviour
    {
        [SerializeField] private BoolValue boolValue;
        [SerializeField] private DateValue arrivalDate;
        [SerializeField] private DateValue departureDate;
        
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
            if (Logic.IsNull(boolValue) || 
                Logic.IsNull(arrivalDate) || 
                Logic.IsNull(departureDate)) return;

            if (boolValue.Value)
                arrivalDate.SetValue(date);
            else
                departureDate.SetValue(date);
        }
    }
}