using System;
using Extensions.ScriptableValues;
using Extensions.UIWindows;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Контроллер взаимодействий с кнопками дней календаря
    /// </summary>
    /// <remarks>
    /// Управляет открытием списка/записи в нормальном режиме
    /// и режимом выбора диапазона дат (зажатие → прибытие, клик → отбытие → Entry edit).
    /// </remarks>
    public sealed class CalendarDayInteractionController : MonoBehaviour
    {
        #region События

        /// <summary>Режим выбора диапазона дат изменился</summary>
        public event Action<bool> onDateRangeModeChanged;

        #endregion

        #region Свойства

        /// <summary>Режим выбора диапазона дат активен (включая ожидание первого клика)</summary>
        public bool IsDateRangeActive => isDateRangeActive || pendingHold;

        #endregion

        #region Параметры

        [Header("Нормальный режим — окна"), Space]
        [SerializeField] private UIWindowID entryWindow;
        [SerializeField] private UIWindowID listWindow;

        [Header("Режим выбора дат — окно и даты"), Space]
        [SerializeField] private UIWindowID editWindow;
        [SerializeField] private DateValue arrivalDate;
        [SerializeField] private DateValue departureDate;
        [SerializeField] private BoolValue dateRangePreset;

        #endregion

        #region Переменные

        private bool isDateRangeActive;
        private bool pendingHold;
        private DateTime pendingArrival;

        private ReservationsMonthViewController calendarController;
        private ReservationsMultipleSelectionContext reservationsContext;
        private UIWindowsController windowsController;
        private UIWindow parentWindow;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            reservationsContext = DataBus.Instance.ReservationsMultipleSelectionContext;
            windowsController = UIWindowsController.Instance;
            parentWindow = GetComponentInParent<UIWindow>();
            calendarController = GetComponent<ReservationsMonthViewController>();
        }

        private void OnEnable() => calendarController.onDayButtonClicked += OnDayButtonClicked;

        private void OnDisable()
        {
            calendarController.onDayButtonClicked -= OnDayButtonClicked;
            DeactivateDateRangeMode();
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Зафиксировать сигнал зажатия кнопки и активировать визуальный индикатор режима
        /// </summary>
        public void SetPendingHold()
        {
            if (pendingHold) return;
            pendingHold = true;
            onDateRangeModeChanged?.Invoke(true);
        }

        /// <summary>
        /// Деактивировать режим выбора диапазона дат
        /// </summary>
        public void DeactivateDateRangeMode()
        {
            if (!isDateRangeActive && !pendingHold) return;
            isDateRangeActive = false;
            pendingHold = false;
            onDateRangeModeChanged?.Invoke(false);
        }

        #endregion

        #region Internal

        private void OnDayButtonClicked(DateTime date)
        {
            if (pendingHold)
            {
                pendingHold = false;
                pendingArrival = date;
                arrivalDate.SetValue(date);
                isDateRangeActive = true;
                return;
            }

            if (isDateRangeActive)
            {
                DeactivateDateRangeMode();
                OpenEditWindowWithDates(pendingArrival, date);
                return;
            }

            OpenListOrEntry();
        }

        private void OpenListOrEntry()
        {
            if (!reservationsContext.HasSelection) return;

            if (reservationsContext.SelectionsNumber == 1)
            {
                DataBus.Instance.ReservationSelectionContext.Select(reservationsContext.SelectedIds[0]);
                windowsController.OpenWindow(entryWindow.Id, parentWindow);
                return;
            }

            windowsController.OpenWindow(listWindow.Id, parentWindow, false);
        }

        private void OpenEditWindowWithDates(DateTime arrival, DateTime departure)
        {
            DataBus.Instance.ReservationSelectionContext.Clear();

            arrivalDate.SetValue(arrival);
            departureDate.SetValue(departure);
            dateRangePreset.SetValue(true);

            windowsController.OpenWindow(editWindow.Id, parentWindow);

            dateRangePreset.SetValue(false);
        }

        #endregion
    }
}
