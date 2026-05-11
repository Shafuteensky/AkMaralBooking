using System;
using System.Collections.Generic;
using Extensions.Log;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Фабрика списка записей аренды
    /// </summary>
    public class ReservationPreviewButtonFactory : GenericDataPreviewButtonFactory<ReservationsDataContainer, ReservationData>
    {
        [SerializeField] private DateValue fromDate;
        [SerializeField] private DateValue toDate;
        
        private HouseSelectionContext houseSelectionContext;

        protected override void Awake()
        {
            base.Awake();
            houseSelectionContext = DataBus.Instance.HouseFilter;
        }
        
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
        
        /// <summary>
        /// Сортировка записей аренды по актуальности
        /// </summary>
        protected override void SortData(List<ReservationData> data)
        {
            data.Sort(CompareByActuality);
        }

        private int CompareByActuality(ReservationData first, ReservationData second)
        {
            if (first == null && second == null) return 0;
            if (first == null) return 1;
            if (second == null) return -1;

            return second.ArrivalDate.CompareTo(first.ArrivalDate);
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