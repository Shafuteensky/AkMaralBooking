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