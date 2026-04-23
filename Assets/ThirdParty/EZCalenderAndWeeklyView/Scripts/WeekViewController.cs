using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace EZCalendarWeeklyView
{
    /// <summary>
    /// Manages the week view UI, allowing navigation and interaction with day buttons.
    /// </summary>
    public class WeekViewController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // UI elements and variables
        public GameObject monthViewPanel;        // Reference to the month view panel
        public List<Button> dayButtons;          // List of day buttons in the week view
        public TextMeshProUGUI selectedDateText; // Text displaying the selected date
        public TextMeshProUGUI monthYearText;    // Text displaying the month and year
        public Color normalColor;                // Normal color for day buttons
        public Color selectedColor;              // Color for the selected day button
        public bool normalSlideDirection = true; // Direction of week slide (left to right)

        // Private variables for date management and touch input
        private DateTime currentDate;            // Currently displayed date
        private DateTime startOfWeek;            // Start date of the current week view
        private List<DateTime> weekDates;        // Dates for each day in the current week
        private DateTime selectedDate;           // Selected date in the week view
        private Vector2 startTouchPosition;      // Initial touch position for swipe detection
        private Vector2 currentTouchPosition;    // Current touch position during drag
        public bool useScreenWidthForSwipe = true;      // Use screen width for swipe threshold
        public float swipeThreshold = 30f;       // Threshold for swipe detection
        

        /// <summary>
        /// Initializes starting values and updates the UI.
        /// </summary>
        void Start()
        {
            if (useScreenWidthForSwipe) swipeThreshold = Screen.width / 1.5f;
            int dayOfWeekIndex = (int)DateTime.Now.DayOfWeek;
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

        /// <summary>
        /// Custom logic executed when a date is clicked.
        /// </summary>
        /// <param name="date">The clicked date.</param>
        private void ClickedDateLogic(DateTime date)
        {
            Debug.Log("Clicked Date: " + date);
            // Add custom logic here as needed
        }

        /// <summary>
        /// Event handler for day button clicks.
        /// </summary>
        /// <param name="index">The index of the clicked button.</param>
        private void OnDayButtonClick(int index)
        {
            selectedDate = weekDates[index];
            currentDate = selectedDate;
            UpdateSelectedDateText(selectedDate);
            HighlightSelectedButton();
            ClickedDateLogic(currentDate);
        }

        /// <summary>
        /// Updates the selected date text on the UI.
        /// </summary>
        /// <param name="date">The selected date.</param>
        private void UpdateSelectedDateText(DateTime date)
        {
            if (selectedDateText != null) selectedDateText.text = date.ToString("MMMM d, yyyy");
            if (monthYearText != null) monthYearText.text = date.ToString("MMMM yyyy");
        }

        /// <summary>
        /// Sets the current date and calculates the start of the week.
        /// </summary>
        /// <param name="date">The date to set.</param>
        public void SetCurDate(DateTime date)
        {
            currentDate = date;
            UpdateSelectedDateText(date);
        }

        /// <summary>
        /// Calculates the start of the week based on a given date.
        /// </summary>
        /// <param name="date">The date to calculate from.</param>
        /// <returns>The start date of the week.</returns>
        private DateTime GetStartOfWeek(DateTime date)
        {
            return date.AddDays(-(int)date.DayOfWeek);
        }

        /// <summary>
        /// Updates the week view with new dates and highlights the selected button.
        /// </summary>
        private void UpdateWeekView()
        {
            weekDates = new List<DateTime>();

            for (int i = 0; i < dayButtons.Count; i++)
            {
                DateTime day = startOfWeek.AddDays(i);
                weekDates.Add(day);

                var dayOfWeekText = dayButtons[i].GetComponentsInChildren<TextMeshProUGUI>(true)
                                                .FirstOrDefault(t => t.CompareTag("dayText"));
                var dateText = dayButtons[i].GetComponentsInChildren<TextMeshProUGUI>(true)
                                            .FirstOrDefault(t => t.CompareTag("dateText"));

                if (dayOfWeekText != null) dayOfWeekText.text = day.DayOfWeek.ToString().Substring(0, 3);
                if (dateText != null) dateText.text = day.ToString("MMM\nd");
            }

            HighlightSelectedButton();
        }

        /// <summary>
        /// Navigates to the previous week and updates the view.
        /// </summary>
        public void GoToPreviousWeek()
        {
            startOfWeek = startOfWeek.AddDays(-7);
            UpdateWeekView();
        }

        /// <summary>
        /// Navigates to the next week and updates the view.
        /// </summary>
        public void GoToNextWeek()
        {
            startOfWeek = startOfWeek.AddDays(7);
            UpdateWeekView();
        }

        /// <summary>
        /// Highlights the selected button based on the selected date.
        /// </summary>
        private void HighlightSelectedButton()
        {
            for (int i = 0; i < dayButtons.Count; i++)
            {
                var colors = dayButtons[i].colors;

                if (weekDates[i] == selectedDate)
                {
                    colors.normalColor = selectedColor;
                    colors.highlightedColor = selectedColor;
                    colors.pressedColor = selectedColor;
                    colors.selectedColor = selectedColor;
                }
                else
                {
                    colors.normalColor = normalColor;
                    colors.highlightedColor = normalColor;
                    colors.pressedColor = normalColor;
                    colors.selectedColor = normalColor;
                }

                dayButtons[i].colors = colors;
            }
        }

        /// <summary>
        /// Begin drag event handler to store start touch position.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            startTouchPosition = eventData.position;
        }

        /// <summary>
        /// Drag event handler to update current touch position and week dates.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnDrag(PointerEventData eventData)
        {
            currentTouchPosition = eventData.position;
            UpdateWeekDatesDuringDrag();
        }

        /// <summary>
        /// End drag event handler to calculate day shift and update week view.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            startOfWeek = startOfWeek.AddDays(CalculateDayShiftForSwipe(true));
            UpdateWeekView();
        }

        /// <summary>
        /// Updates week dates during drag to simulate swipe motion.
        /// </summary>
        private void UpdateWeekDatesDuringDrag()
        {
            int dayShift = CalculateDayShiftForSwipe(false);
            if (normalSlideDirection) dayShift = -dayShift;

            List<DateTime> tempWeekDates = new List<DateTime>();

            for (int i = 0; i < dayButtons.Count; i++)
            {
                DateTime day = startOfWeek.AddDays(i + dayShift);
                tempWeekDates.Add(day);

                var dayOfWeekText = dayButtons[i].GetComponentsInChildren<TextMeshProUGUI>(true)
                                                .FirstOrDefault(t => t.CompareTag("dayText"));
                var dateText = dayButtons[i].GetComponentsInChildren<TextMeshProUGUI>(true)
                                            .FirstOrDefault(t => t.CompareTag("dateText"));

                if (dayOfWeekText != null) dayOfWeekText.text = day.DayOfWeek.ToString().Substring(0, 3);
                if (dateText != null) dateText.text = day.ToString("MMM\nd");
            }

            weekDates = tempWeekDates;
            HighlightSelectedButton();
        }

        /// <summary>
        /// Calculates day shift based on swipe distance for swipe motion.
        /// </summary>
        /// <param name="isEndDrag">Whether it is the end of the drag.</param>
        /// <returns>The day shift for swipe motion.</returns>
        private int CalculateDayShiftForSwipe(bool isEndDrag)
        {
            float swipeDistance = currentTouchPosition.x - startTouchPosition.x;
            int dayShift = Mathf.Clamp(Mathf.RoundToInt(7 * (swipeDistance / swipeThreshold)), -7, 7);
            return isEndDrag ? (normalSlideDirection ? -dayShift : dayShift) : (normalSlideDirection ? dayShift : dayShift);
        }

        /// <summary>
        /// Navigates to today's week and updates the view.
        /// </summary>
        public void GoToToday()
        {
            DateTime today = DateTime.Now;
            startOfWeek = GetStartOfWeek(today);
            UpdateWeekView();
            OnDayButtonClick((int)today.DayOfWeek);
        }

        /// <summary>
        /// Shows the associated month view panel.
        /// </summary>
        public void ShowMonthView()
        {
            monthViewPanel.SetActive(true);
            monthViewPanel.GetComponent<MonthViewController>().ShowMonthView(currentDate);
        }

        /// <summary>
        /// Hides the associated month view panel.
        /// </summary>
        public void HideMonthView()
        {
            monthViewPanel.SetActive(false);
            monthViewPanel.GetComponent<MonthViewController>().HideMonthView();
        }

        /// <summary>
        /// Navigates to a specific date's week and updates the view.
        /// </summary>
        /// <param name="date">The date to navigate to.</param>
        public void GoToDate(DateTime date)
        {
            selectedDate = date;
            startOfWeek = GetStartOfWeek(date);
            UpdateWeekView();
        }
    }
}