using System.Collections.Generic;
using StarletBooking.Data;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Фабрика списка записей аренды за выбранный день
    /// </summary>
    public class ReservationSelectionPreviewButtonFactory : GenericDataPreviewButtonFactory<ReservationsDataContainer, ReservationData>
    {
        private ReservationsMultipleSelectionContext reservationsMultipleSelectionContext;
        
        protected override void Awake()
        {
            base.Awake();
            reservationsMultipleSelectionContext = DataBus.Instance.ReservationsMultipleSelectionContext;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            reservationsMultipleSelectionContext.onSelectionChanged += Rebuild;
        }
        
        protected void OnDisable()
        {
            reservationsMultipleSelectionContext.onSelectionChanged -= Rebuild;
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
            if (!reservationsMultipleSelectionContext.HasSelection) 
                return false;
            
            foreach (var reservationId in reservationsMultipleSelectionContext.SelectedIds)
                if (data.Id == reservationId) return true;
            
            return false;
        }
    }
}