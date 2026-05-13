using System;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Индикатор выбора текущего месяца в календаре
    /// </summary>
    [RequireComponent(typeof(ReservationsMonthViewController))]
    public class CurrentMonthIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject indicator;
        
        private ReservationsMonthViewController calendarController;

        private void Awake()
        {
            calendarController = GetComponent<ReservationsMonthViewController>();
        }

        private void OnEnable()
        {
            calendarController.onCalendarUpdated += UpdateIndicatorState;
        }

        private void OnDisable()
        {
            calendarController.onCalendarUpdated -= UpdateIndicatorState;
        }

        private void UpdateIndicatorState()
        {
            indicator.SetActive(IsThisMonth());
        }

        private bool IsThisMonth()
        {
            DateTime currentDate = calendarController.CurrentDate;
            return currentDate.Month == DateTime.Today.Month && currentDate.Year == DateTime.Today.Year;
        }
    }
}