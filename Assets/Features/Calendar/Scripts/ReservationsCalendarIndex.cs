using System;
using System.Collections.Generic;
using Extensions.Data.InMemoryData;
using StarletBooking.Data;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Индекс записей аренды <see cref="ReservationData"/> по датам календаря
    /// </summary>
    public sealed class ReservationsCalendarIndex
    {
        private readonly Dictionary<DateTime, List<ReservationData>> reservationsByDate = new();
        private readonly List<ReservationData> emptyReservations = new();

        /// <summary>
        /// Очистить индекс
        /// </summary>
        public void Clear() => reservationsByDate.Clear();

        /// <summary>
        /// Пересобрать индекс
        /// </summary>
        /// <param name="reservationsContainer">Контейнер записей аренды</param>
        /// <param name="houseId">Идентификатор дома</param>
        /// <param name="hasHouseFilter">Использовать фильтр дома</param>
        /// <param name="filterArrivalDate">Дата прибытия фильтра</param>
        /// <param name="hasArrivalDateFilter">Использовать фильтр даты прибытия</param>
        /// <param name="filterDepartureDate">Дата отбытия фильтра</param>
        /// <param name="hasDepartureDateFilter">Использовать фильтр даты отбытия</param>
        public void Rebuild(
            InMemoryDataContainer<ReservationData> reservationsContainer,
            string houseId,
            bool hasHouseFilter,
            DateTime filterArrivalDate,
            bool hasArrivalDateFilter,
            DateTime filterDepartureDate,
            bool hasDepartureDateFilter)
        {
            reservationsByDate.Clear();

            if (reservationsContainer == null) return;

            DateTime filterStart = filterArrivalDate.Date;
            DateTime filterEnd = filterDepartureDate.Date;

            if (hasArrivalDateFilter && hasDepartureDateFilter && filterStart > filterEnd) return;

            List<ReservationData> reservations = reservationsContainer.Data;

            foreach (var reservation in reservations)
            {
                if (reservation == null) continue;
                if (hasHouseFilter && reservation.HouseId != houseId) continue;

                DateTime reservationStart = reservation.ArrivalDate.Date;
                DateTime reservationEnd = reservation.DepartureDate.Date;

                if (reservationStart > reservationEnd) continue;
                if (!PassesDateFilter(reservationStart, reservationEnd, filterStart, hasArrivalDateFilter, filterEnd, hasDepartureDateFilter)) continue;

                AddReservation(reservation, reservationStart, reservationEnd);
            }
        }

        /// <summary>
        /// Получить записи аренды по дате
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Список записей аренды</returns>
        public IReadOnlyList<ReservationData> GetReservations(DateTime date)
        {
            DateTime key = date.Date;

            if (reservationsByDate.TryGetValue(key, out List<ReservationData> reservations)) return reservations;

            return emptyReservations;
        }

        private void AddReservation(ReservationData reservation, DateTime reservationStart, DateTime reservationEnd)
        {
            DateTime date = reservationStart.Date;

            while (date <= reservationEnd)
            {
                if (!reservationsByDate.TryGetValue(date, out List<ReservationData> reservations))
                {
                    reservations = new List<ReservationData>();
                    reservationsByDate.Add(date, reservations);
                }

                reservations.Add(reservation);
                date = date.AddDays(1);
            }
        }

        private bool PassesDateFilter(
            DateTime reservationStart,
            DateTime reservationEnd,
            DateTime filterStart,
            bool hasArrivalDateFilter,
            DateTime filterEnd,
            bool hasDepartureDateFilter)
        {
            if (!hasArrivalDateFilter && !hasDepartureDateFilter) return true;
            if (hasArrivalDateFilter && reservationEnd < filterStart) return false;
            if (hasDepartureDateFilter && reservationStart > filterEnd) return false;

            return true;
        }
    }
}