using System;
using EZCalendarWeeklyView;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Недельный календарь записей аренды
    /// </summary>
    public sealed class ReservationsWeekViewController : WeekViewController
    {
        protected override void Start()
        {
            if (useScreenWidthForSwipe) swipeThreshold = Screen.width / 1.5f;

            int dayOfWeekIndex = GetWeekDayIndex(DateTime.Now);

            currentDate = DateTime.Now;
            selectedDate = DateTime.MinValue;
            startOfWeek = GetStartOfWeek(currentDate);
            UpdateWeekView();

            for (int i = 0; i < dayButtons.Count; i++)
            {
                int index = i;
                dayButtons[i].onClick.AddListener(() => OnDayButtonClick(index));
            }

            OnDayButtonClick(dayOfWeekIndex);
        }
        
        public override void GoToToday()
        {
            DateTime today = DateTime.Now;
            startOfWeek = GetStartOfWeek(today);
            UpdateWeekView();
            OnDayButtonClick(GetWeekDayIndex(today));
        }
        
        protected override DateTime GetStartOfWeek(DateTime date) => date.Date.AddDays(-GetWeekDayIndex(date));
        
        private int GetWeekDayIndex(DateTime date) => ((int)date.DayOfWeek + 6) % 7;
    }
}