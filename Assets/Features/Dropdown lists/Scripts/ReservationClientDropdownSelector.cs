using StarletBooking.Data;

namespace StarletBooking.UI
{
    /// <summary>
    /// Селектор выбора дропдаун-списка клиента из записи аренды
    /// </summary>
    public class ReservationClientDropdownSelector : ReservationDropdownSelector
    {
        protected override string GetSelected(ReservationSelectionContext reservationSelectionContext) 
            => reservationSelectionContext.GetSelectedData().ClientId;
    }
}