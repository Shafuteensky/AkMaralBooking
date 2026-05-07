using System;
using Extensions.Log;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Фабрика списка записей аренды
    /// </summary>
    public class ReservationPreviewButtonFactory : GenericDataPreviewButtonFactory<ReservationsDataContainer, ReservationData>
    {
        [FormerlySerializedAs("houseSingleSelectionContext")] [SerializeField] private HouseSelectionContext houseSelectionContext;
        [SerializeField] private DateValue fromDate;
        [SerializeField] private DateValue toDate;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (applyFilter)
            {
                houseSelectionContext.onSelectionChanged += Rebuild;
                fromDate.onValueChanged += Rebuild;
                toDate.onValueChanged += Rebuild;
            }
        }
        
        protected void OnDisable()
        {
            houseSelectionContext.onSelectionChanged -= Rebuild;
            fromDate.onValueChanged -= Rebuild;
            toDate.onValueChanged -= Rebuild;
        }

        protected override bool FilterCheck(ReservationData data)
        {
            if (!houseSelectionContext ||
                fromDate == null || toDate == null)
            {
                ServiceDebug.LogError("Компоненты фильтрации не назначены, фильтрация не выполнена");
                return true;
            }

            return IsFilteredByHouse(data) && IsFilteredByDate(data);
        }

        private bool IsFilteredByHouse(ReservationData data)
        {
            bool isDataValid = houseSelectionContext.TryGetSelectedData(out HouseData houseData);
            
            if (!isDataValid || data.HouseId == houseData.Id) 
                return true;
            else 
                return false;
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

        private void Rebuild(string _) => Rebuild();
    }
}