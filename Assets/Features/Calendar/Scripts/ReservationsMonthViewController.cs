using System;
using System.Collections.Generic;
using Extensions.ScriptableValues;
using EZCalendarWeeklyView;
using StarletBooking.Data;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Месячный календарь записей аренды
    /// </summary>
    public sealed class ReservationsMonthViewController : MonthViewController
    {
        [Header("Данные записей аренды")]
        [SerializeField] private ReservationsDataContainer reservationsDataContainer;

        [Header("Выбранные записи")]
        [SerializeField] private ReservationSelectionContext reservationSelectionContext;
        [SerializeField] private ReservationsMultipleSelectionContext reservationsMultipleSelectionContext;

        [Header("Фильтрация")]
        [SerializeField] private HouseSelectionContext houseSelectionContext;
        [SerializeField] private DateValue arrivalDateFilter;
        [SerializeField] private DateValue departureDateFilter;

        private readonly ReservationsCalendarIndex reservationsIndex = new ReservationsCalendarIndex();
        private readonly List<Color> reservationColors = new List<Color>();
        private readonly List<string> reservationIds = new List<string>();

        protected override void Start()
        {
            RebuildReservationsIndex();
            base.Start();
        }

        /// <summary>
        /// Обновить календарь записей аренды
        /// </summary>
        public void RefreshReservationsCalendar()
        {
            RebuildReservationsIndex();
            UpdateMonthView();
        }

        protected override void UpdateDayButton(Button button, DateTime date, Color color)
        {
            base.UpdateDayButton(button, date, color);

            IReadOnlyList<ReservationData> reservations = reservationsIndex.GetReservations(date);
            UpdateDayView(button, reservations);
            UpdateDayClick(button, date, reservations);
        }

        private void RebuildReservationsIndex()
        {
            reservationsIndex.Clear();

            if (reservationsDataContainer == null ||
                houseSelectionContext == null || 
                !houseSelectionContext.HasSelection || 
                arrivalDateFilter == null || 
                !arrivalDateFilter.TryGetDate(out DateTime arrivalDate) || 
                departureDateFilter == null || 
                !departureDateFilter.TryGetDate(out DateTime departureDate))
            {
                return;
            }

            reservationsIndex.Rebuild(
                reservationsDataContainer,
                houseSelectionContext.SelectedId,
                arrivalDate,
                departureDate
            );
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

            button.onClick.AddListener(() => SelectReservations(reservations));
        }

        private void OnEmptyDayClick(DateTime date)
        {
            reservationSelectionContext.Clear();
            reservationsMultipleSelectionContext.Clear();

            OnDayButtonClick(date);
        }

        private void SelectReservations(IReadOnlyList<ReservationData> reservations)
        {
            if (reservations.Count == 1)
            {
                reservationsMultipleSelectionContext.Clear();
                reservationSelectionContext.Select(reservations[0].Id);
                return;
            }

            reservationSelectionContext.Clear();

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

            foreach (var t in reservations)
            {
                if (t == null) continue;

                ClientData client = reservationsDataContainer.GetClientById(t.Id);
                if (client == null) continue;

                reservationColors.Add(client.Color);
            }
        }
    }
}