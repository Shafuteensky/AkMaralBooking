using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace EZCalendarWeeklyView
{
    /// <summary>
    /// Controls the month view for a calendar, allowing navigation between months and years,
    /// highlighting selected dates, and managing day buttons.
    /// </summary>
    public class MonthViewController : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public WeekViewController weekView;           // Reference to the WeekViewController
        public DynamicGridLayout dynamicGridLayout;   // DynamicGridLayout for adjusting grid layout
        public List<Button> dayButtons;               // List of buttons representing days in the calendar
        public TextMeshProUGUI monthYearText;         // Text to display the current month and year

        public Color normalDayColor;                  // Color for normal day buttons
        public Color fadedDayColor;                   // Color for days not in the current month
        public Color todayHighlightColor;             // Color for highlighting today's date

        private DateTime currentDate;                 // Current date displayed in the calendar
        private DateTime selectedDate;                // Currently selected date
        private bool doneSwiping;                     // Flag to indicate if swiping is done

        public float swipeThreshold = 30;             // Threshold for detecting a swipe
        public bool normalSlideDirection = true;              // Flag for normal swipe behavior

        private Dictionary<Button, DateTime> buttonDates = new Dictionary<Button, DateTime>(); // Dictionary to store date information for each button

        /// <summary>
        /// Initializes the current and selected dates and shows the month view.
        /// </summary>
        void Start()
        {
            if (currentDate == DateTime.MinValue) currentDate = DateTime.Now;
            if (selectedDate == DateTime.MinValue) selectedDate = DateTime.Now;
            ShowMonthView(currentDate);
        }

        /// <summary>
        /// Custom method for handling date click in month view only mode.
        /// </summary>
        /// <param name="date">The date that was clicked.</param>
        private void MonthOnlyCustomMethod(DateTime date)
        {
            Debug.Log("Clicked Date: " + date);

            var textToChange = GameObject.Find("TextToChange");
            if (textToChange != null)
            {
                currentDate = date;
                selectedDate = date;
                UpdateMonthView();
                textToChange.GetComponent<TextMeshProUGUI>().text = date.ToString("MMMM d, yyyy");
            }
        }

        /// <summary>
        /// Shows the month view for the specified date.
        /// </summary>
        /// <param name="date">The date to display in the month view.</param>
        public void ShowMonthView(DateTime date)
        {
            currentDate = date;
            selectedDate = date;
            gameObject.SetActive(true);
            UpdateMonthView();
        }

        /// <summary>
        /// Hides the month view if the faded outside are is clicked AND weekView exists
        /// </summary>

        /// <summary>
        /// Hides the month view.
        /// </summary>
        public void HideMonthView()
        {
            if (weekView != null)
            {
                gameObject.SetActive(false);
            }   else
            {
                Debug.Log("WeekView is null, edit this code to hide the month view.");
            }
        }

        /// <summary>
        /// Updates the month view to reflect the current date.
        /// </summary>
        public void UpdateMonthView()
        {
            monthYearText.text = currentDate.ToString("MMMM yyyy");
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            var previousMonth = currentDate.AddMonths(-1);
            int daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);

            UpdateButtonsForPreviousMonth(daysInPreviousMonth, startDayOfWeek);
            UpdateButtonsForCurrentMonth(daysInMonth, startDayOfWeek);
            UpdateButtonsForNextMonth(startDayOfWeek, daysInMonth);
            dynamicGridLayout.UpdateGridLayout();
        }

        /// <summary>
        /// Updates buttons for the days of the previous month.
        /// </summary>
        /// <param name="daysInPreviousMonth">The number of days in the previous month.</param>
        /// <param name="startDayOfWeek">The starting day of the week for the current month.</param>
        void UpdateButtonsForPreviousMonth(int daysInPreviousMonth, int startDayOfWeek)
        {
            for (int i = 0; i < startDayOfWeek; i++)
            {
                int day = daysInPreviousMonth - startDayOfWeek + i + 1;
                var date = new DateTime(currentDate.AddMonths(-1).Year, currentDate.AddMonths(-1).Month, day);
                UpdateDayButton(dayButtons[i], date, fadedDayColor);
            }
        }

        /// <summary>
        /// Updates buttons for the days of the current month.
        /// </summary>
        /// <param name="daysInMonth">The number of days in the current month.</param>
        /// <param name="startDayOfWeek">The starting day of the week for the current month.</param>
        void UpdateButtonsForCurrentMonth(int daysInMonth, int startDayOfWeek)
        {
            for (int i = 0; i < daysInMonth; i++)
            {
                int day = i + 1;
                var date = new DateTime(currentDate.Year, currentDate.Month, day);
                UpdateDayButton(dayButtons[startDayOfWeek + i], date, normalDayColor);

                if (date.Date == selectedDate.Date)
                {
                    HighlightButton(dayButtons[startDayOfWeek + i], todayHighlightColor);
                }
                else
                {
                    UpdateDayButton(dayButtons[startDayOfWeek + i], date, normalDayColor);
                }
            }
        }

        /// <summary>
        /// Updates buttons for the days of the next month.
        /// </summary>
        /// <param name="startDayOfWeek">The starting day of the week for the current month.</param>
        /// <param name="daysInMonth">The number of days in the current month.</param>
        void UpdateButtonsForNextMonth(int startDayOfWeek, int daysInMonth)
        {
            int nextMonthStartIndex = startDayOfWeek + daysInMonth;
            for (int i = nextMonthStartIndex; i < dayButtons.Count; i++)
            {
                int day = i - nextMonthStartIndex + 1;
                var date = new DateTime(currentDate.AddMonths(1).Year, currentDate.AddMonths(1).Month, day);
                UpdateDayButton(dayButtons[i], date, fadedDayColor);
            }
        }

        /// <summary>
        /// Updates a day button with the given date and color.
        /// </summary>
        /// <param name="button">The button to update.</param>
        /// <param name="date">The date to assign to the button.</param>
        /// <param name="color">The color to set for the button.</param>
        void UpdateDayButton(Button button, DateTime date, Color color)
        {
            var dateText = button.GetComponentInChildren<TextMeshProUGUI>();
            dateText.text = date.Day.ToString();

            // Store the date information for this button
            if (buttonDates.ContainsKey(button))
            {
                buttonDates[button] = date;
            }
            else
            {
                buttonDates.Add(button, date);
            }

            // Determine appropriate color based on month
            color = date.Month == currentDate.Month && date.Year == currentDate.Year ? normalDayColor : fadedDayColor;

            // Update button colors
            var colors = button.colors;
            colors.normalColor = color;
            colors.selectedColor = color;
            button.colors = colors;

            // Add click listener
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnDayButtonClick(date));
        }

        /// <summary>
        /// Highlights a button with the specified color.
        /// </summary>
        /// <param name="button">The button to highlight.</param>
        /// <param name="color">The color to use for highlighting.</param>
        void HighlightButton(Button button, Color color)
        {
            var colors = button.colors;
            colors.normalColor = color;
            colors.selectedColor = color;
            button.colors = colors;
        }

        /// <summary>
        /// Handles the click event for a day button.
        /// </summary>
        /// <param name="date">The date that was clicked.</param>
        void OnDayButtonClick(DateTime date)
        {
            selectedDate = date;
            currentDate = date;

            if (weekView != null)
            {
                weekView.SetCurDate(date);
                weekView.GoToDate(date);
                HideMonthView();
            }
            else
            {
                DeselectAllDayButtons(); // Deselect all buttons first
                MonthOnlyCustomMethod(date);
            }

            UpdateMonthView(); // Highlight the clicked button
        }

        /// <summary>
        /// Navigates to the previous month and updates the month view.
        /// </summary>
        public void GoToPreviousMonth()
        {
            doneSwiping = true;
            currentDate = currentDate.AddMonths(-1);
            UpdateMonthView();
        }

        /// <summary>
        /// Navigates to the next month and updates the month view.
        /// </summary>
        public void GoToNextMonth()
        {
            doneSwiping = true;
            currentDate = currentDate.AddMonths(1);
            UpdateMonthView();
        }

        /// <summary>
        /// Navigates to the previous year and updates the month view.
        /// </summary>
        public void GoToPreviousYear()
        {
            doneSwiping = true;
            currentDate = currentDate.AddYears(-1);
            UpdateMonthView();
        }

        /// <summary>
        /// Navigates to the next year and updates the month view.
        /// </summary>
        public void GoToNextYear()
        {
            doneSwiping = true;
            currentDate = currentDate.AddYears(1);
            UpdateMonthView();
        }

        /// <summary>
        /// Handles the drag event for swiping between months.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnDrag(PointerEventData eventData)
        {
            if (doneSwiping) return;

            float dragDistance = eventData.delta.x;
            if (dragDistance > swipeThreshold)
            {
                if (normalSlideDirection) GoToPreviousMonth();
                else GoToNextMonth();
            }
            else if (dragDistance < -swipeThreshold)
            {
                if (normalSlideDirection) GoToNextMonth();
                else GoToPreviousMonth();
            }
        }

        /// <summary>
        /// Handles the end drag event for swiping between months.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            doneSwiping = false;
        }

        /// <summary>
        /// Navigates to today's date and updates the month view.
        /// </summary>
        public void GoToToday()
        {
            currentDate = DateTime.Now;
            selectedDate = DateTime.Now;
            OnDayButtonClick(DateTime.Now);
            UpdateMonthView();
        }

        /// <summary>
        /// Deselects all day buttons by resetting their colors to normal.
        /// </summary>
        void DeselectAllDayButtons()
        {
            foreach (var entry in buttonDates)
            {
                Button button = entry.Key;
                DateTime date = entry.Value;

                // Determine appropriate color based on month
                Color color = date.Month == currentDate.Month && date.Year == currentDate.Year ? normalDayColor : fadedDayColor;

                // Update button colors
                var colors = button.colors;
                colors.normalColor = color;
                colors.selectedColor = color;
                button.colors = colors;
            }
        }
    }
}