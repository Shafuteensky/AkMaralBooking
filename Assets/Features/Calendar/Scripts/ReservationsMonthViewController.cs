using System;
using System.Collections.Generic;
using Extensions.Coroutines;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using EZCalendarWeeklyView;
using StarletBooking.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DynamicGridLayout = Extensions.Layouts.DynamicGridLayout;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Месячный календарь записей аренды
    /// </summary>
    public sealed class ReservationsMonthViewController : MonthViewController
    {
        #region События
        
        /// <summary>
        /// Событие изменения месяца назад от текущего
        /// </summary>
        public event Action onPreviousMonthChanged;
        /// <summary>
        /// Событие изменения месяца вперед от текущего
        /// </summary>
        public event Action onNextMonthChanged;
        /// <summary>
        /// Событие отрисовки календаря
        /// </summary>
        public event Action onCalendarUpdated;
        
        #endregion
        
        #region Параметры
        
        [SerializeField] private DynamicGridLayout newDynamicGridLayout;   
        
        [Header("Данные записей аренды")]
        [SerializeField] private ReservationsDataContainer reservationsDataContainer;
        [SerializeField] private ReservationsMultipleSelectionContext reservationsMultipleSelectionContext;

        [FormerlySerializedAs("houseSingleSelectionContext")]
        [Header("Фильтрация")]
        [SerializeField] private HouseSelectionContext houseSelectionContext;
        [SerializeField] private DateValue arrivalDateFilter;
        [SerializeField] private DateValue departureDateFilter;

        private readonly ReservationsCalendarIndex reservationsIndex = new ReservationsCalendarIndex();
        private readonly List<Color> reservationColors = new();
        private readonly List<string> reservationIds = new();
        
        #endregion

        #region MonoBehavior
        
        private void Awake()
        {
            currentDate = DateTime.Now;
            selectedDate = DateTime.Now;
        }
        
        protected override void Start()
        {
            RebuildReservationsIndex();
            base.Start();
            RefreshReservationsCalendar();
            CoroutineDelay.Run(this, newDynamicGridLayout.UpdateGridLayout);
        }
        
        #endregion

        #region Управление вьюшкой

        public override void GoToPreviousMonth()
        {
            base.GoToPreviousMonth();
            onPreviousMonthChanged?.Invoke();
        }

        public override void GoToNextMonth()
        {
            base.GoToNextMonth();
            onNextMonthChanged?.Invoke();
        }

        public override void GoToPreviousYear()
        {
            base.GoToPreviousYear();
            onPreviousMonthChanged?.Invoke();
        }

        public override void GoToNextYear()
        {
            base.GoToNextYear();
            onNextMonthChanged?.Invoke();
        }

        public override void GoToToday()
        {
            DateTime previousDate = currentDate;
            DateTime newDate = DateTime.Now;
            
            base.GoToToday();

            RaiseMonthChangedByDate(previousDate, newDate);
        }

        protected override void OnDayButtonClick(DateTime date)
        {
            DateTime previousDate = currentDate;
            DateTime newDate = date;
            
            base.OnDayButtonClick(date);
            
            RaiseMonthChangedByDate(previousDate, newDate);
        }

        private void RaiseMonthChangedByDate(DateTime previousDate, DateTime newDate)
        {
            int monthOffset = (newDate.Year - previousDate.Year) * 12 + newDate.Month - previousDate.Month;
            if (monthOffset < 0) onPreviousMonthChanged?.Invoke();
            else if (monthOffset > 0) onNextMonthChanged?.Invoke();
        }
        
        #endregion
        
        #region Получение данных

        /// <summary>
        /// Получить диапазон дат, видимый в месячном календаре
        /// </summary>
        public void GetVisibleDateRange(out DateTime firstVisibleDate, out DateTime lastVisibleDate)
        {
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int startDayOfWeek = GetWeekDayIndex(firstDayOfMonth);

            firstVisibleDate = firstDayOfMonth.AddDays(-startDayOfWeek).Date;
            lastVisibleDate = firstVisibleDate.AddDays(dayButtons.Count - 1).Date;
        }
        
        #endregion

        #region Обновление календаря и его элементов
        
        /// <summary>
        /// Обновить календарь записей аренды
        /// </summary>
        public void RefreshReservationsCalendar()
        {
            RebuildReservationsIndex();
            UpdateMonthView();
        }

        public override void UpdateMonthView()
        {
            monthYearText.text = Formatters.FormatString(currentDate.ToString("MMMM yyyy"));
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDayOfWeek = GetWeekDayIndex(firstDayOfMonth);

            var previousMonth = currentDate.AddMonths(-1);
            int daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);

            UpdateButtonsForPreviousMonth(daysInPreviousMonth, startDayOfWeek);
            UpdateButtonsForCurrentMonth(daysInMonth, startDayOfWeek);
            UpdateButtonsForNextMonth(startDayOfWeek, daysInMonth);
            
            newDynamicGridLayout.UpdateGridLayout();
            onCalendarUpdated?.Invoke();
        }
        
        protected override void UpdateDayButton(Button button, DateTime date, Color color)
        {
            base.UpdateDayButton(button, date, color);

            IReadOnlyList<ReservationData> reservations = reservationsIndex.GetReservations(date);
            UpdateDayView(button, reservations);
            UpdateDayClick(button, date, reservations);
        }

        private void UpdateDayView(Button button, IReadOnlyList<ReservationData> reservations)
        {
            ReservationCalendarDayView dayView = button.GetComponent<ReservationCalendarDayView>();

            if (dayView == null) return;

            FillReservationColors(reservations);
            dayView.SetColors(reservationColors);
        }

        private void UpdateDayClick(Button button, DateTime date, IReadOnlyList<ReservationData> reservations)
        {
            button.onClick.RemoveAllListeners();

            if (reservations == null || reservations.Count == 0)
            {
                button.onClick.AddListener(() => OnEmptyDayClick(date));
                return;
            }

            button.onClick.AddListener(() => OnReservationDayClick(date, reservations));
        }
        
        #endregion

        #region Выполнение при нажатии на кнопки дня
        
        private void OnEmptyDayClick(DateTime date)
        {
            reservationsMultipleSelectionContext.Clear();

            OnDayButtonClick(date);
        }
        
        private void OnReservationDayClick(DateTime date, IReadOnlyList<ReservationData> reservations)
        {
            SelectReservations(reservations);
            OnDayButtonClick(date);
        }
        
        #endregion

        #region Тачскрин
        
        public override void OnDrag(PointerEventData eventData)
        {
            if (doneSwiping) return;

            Vector2 dragDelta = eventData.delta;

            if (Mathf.Abs(dragDelta.x) >= Mathf.Abs(dragDelta.y))
            {
                HandleHorizontalDrag(dragDelta.x);
                return;
            }

            HandleVerticalDrag(dragDelta.y);
        }
        
        private void HandleHorizontalDrag(float dragDistance)
        {
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

        private void HandleVerticalDrag(float dragDistance)
        {
            if (dragDistance > swipeThreshold)
            {
                if (normalSlideDirection) GoToNextMonth();
                else GoToPreviousMonth();
            }
            else if (dragDistance < -swipeThreshold)
            {
                if (normalSlideDirection) GoToPreviousMonth();
                else GoToNextMonth();
            }
        }
        
        #endregion
        
        #region Внутренние функции

        protected override void MonthOnlyCustomMethod(DateTime date) { }
        
        private void SelectReservations(IReadOnlyList<ReservationData> reservations)
        {
            reservationIds.Clear();

            foreach (var t in reservations)
            {
                if (t == null || 
                    string.IsNullOrEmpty(t.Id))
                {
                    continue;
                }

                reservationIds.Add(t.Id);
            }

            reservationsMultipleSelectionContext.Select(reservationsDataContainer, reservationIds);
        }

        private void FillReservationColors(IReadOnlyList<ReservationData> reservations)
        {
            reservationColors.Clear();

            if (reservations == null) return;

            foreach (var reservation in reservations)
            {
                if (reservation == null) continue;

                if (!houseSelectionContext.HasSelection)
                {
                    HouseData house = reservationsDataContainer.GetHouseById(reservation.Id);
                    if (house == null) continue;

                    reservationColors.Add(house.Color);
                }
                else
                {
                    ClientData client = reservationsDataContainer.GetClientById(reservation.Id);
                    if (client == null) continue;

                    reservationColors.Add(client.Color);
                }
            }
        }
        
        
        private void RebuildReservationsIndex()
        {
            reservationsIndex.Clear();

            if (reservationsDataContainer == null) return;

            string houseId = string.Empty;
            bool hasHouseFilter = houseSelectionContext != null && 
                                  houseSelectionContext.HasSelection && 
                                  !string.IsNullOrEmpty(houseSelectionContext.SelectedId);

            if (hasHouseFilter)
            {
                houseId = houseSelectionContext.SelectedId;
            }

            DateTime arrivalDate = default;
            DateTime departureDate = default;

            bool hasArrivalDateFilter = arrivalDateFilter != null && arrivalDateFilter.TryGetDate(out arrivalDate);
            bool hasDepartureDateFilter = departureDateFilter != null && departureDateFilter.TryGetDate(out departureDate);

            reservationsIndex.Rebuild(
                reservationsDataContainer,
                houseId,
                hasHouseFilter,
                arrivalDate,
                hasArrivalDateFilter,
                departureDate,
                hasDepartureDateFilter
            );
        }
        
        private int GetWeekDayIndex(DateTime date) => ((int)date.DayOfWeek + 6) % 7;
        
        #endregion
    }
}