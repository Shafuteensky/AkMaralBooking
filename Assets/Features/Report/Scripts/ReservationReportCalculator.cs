using System;
using System.Collections.Generic;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.Report
{
    /// <summary>
    /// Вычисляет агрегированные показатели аренды с учётом фильтров по дому и датам
    /// </summary>
    public class ReservationReportCalculator : MonoBehaviour
    {
        [SerializeField] private ReservationsDataContainer reservations;
        [SerializeField] private HouseSelectionContext houseFilter;
        [SerializeField] private DateValue fromDate;
        [SerializeField] private DateValue toDate;

        public event Action<ReservationReportData> onReportCalculated;

        private void OnEnable()
        {
            houseFilter.onSelectionChanged += Calculate;
            fromDate.onValueChanged += Calculate;
            toDate.onValueChanged += Calculate;
            Calculate();
        }

        private void OnDisable()
        {
            houseFilter.onSelectionChanged -= Calculate;
            fromDate.onValueChanged -= Calculate;
            toDate.onValueChanged -= Calculate;
        }

        private void Calculate(string _) => Calculate();

        public void Calculate()
        {
            if (Logic.IsNull(reservations, nameof(reservations))) return;
            if (Logic.IsNull(houseFilter, nameof(houseFilter))) return;
            if (Logic.IsNull(fromDate, nameof(fromDate))) return;
            if (Logic.IsNull(toDate, nameof(toDate))) return;

            var report = new ReservationReportData();
            var uniqueClients = new HashSet<string>();

            foreach (var reservation in reservations.Data)
            {
                if (!IsFilteredByHouse(reservation)) continue;
                if (!IsFilteredByDate(reservation)) continue;

                report.ReservationsCount++;
                uniqueClients.Add(reservation.ClientId);
                report.TotalDays += reservation.Days;

                float gross = reservation.PaymentPerDay * reservation.Days;
                float net = gross - reservation.Discount;
                float rate = reservation.ExchangeRate;

                report.GrossProfitUsd += gross;
                report.GrossProfit += gross * rate;
                report.TotalDiscountUsd += reservation.Discount;
                report.TotalDiscount += reservation.Discount * rate;
                report.NetProfitUsd += net;
                report.NetProfit += net * rate;
                report.TotalPrepaymentUsd += reservation.Prepayment;
                report.TotalPrepayment += reservation.Prepayment * rate;
            }

            report.UniqueClientsCount = uniqueClients.Count;
            report.RemainingPaymentUsd = report.NetProfitUsd - report.TotalPrepaymentUsd;
            report.RemainingPayment = report.NetProfit - report.TotalPrepayment;

            onReportCalculated?.Invoke(report);
        }

        private bool IsFilteredByHouse(ReservationData data)
        {
            bool isDataValid = houseFilter.TryGetSelectedData(out HouseData houseData);
            return !isDataValid || data.HouseId == houseData.Id;
        }

        private bool IsFilteredByDate(ReservationData data)
        {
            bool hasFromDate = fromDate.TryGetDate(out DateTime filterFromDate);
            bool hasToDate = toDate.TryGetDate(out DateTime filterToDate);
            if (!hasFromDate && !hasToDate) return true;

            DateTime reservationFromDate = data.ArrivalDate.Date;
            DateTime reservationToDate = data.DepartureDate.Date;

            DateTime from = hasFromDate ? filterFromDate.Date : DateTime.MinValue.Date;
            DateTime to = hasToDate ? filterToDate.Date : DateTime.MaxValue.Date;

            if (from > to) return true;
            return reservationFromDate <= to && reservationToDate >= from;
        }
    }
}
