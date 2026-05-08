using Extensions.Data.InMemoryData.SelectionContext;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Удаление данных о записи аренды
    /// </summary>
    public class DeleteReservationSelectionButton : DeleteSelectionContextButtonBase<ReservationData>
    {
        protected override SingleSelectionContext<ReservationData> singleSelectionContext => DataBus.Instance.ReservationSelectionContext;
    }
}