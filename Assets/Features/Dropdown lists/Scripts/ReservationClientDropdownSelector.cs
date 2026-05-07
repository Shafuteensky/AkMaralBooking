using StarletBooking.Data;

namespace StarletBooking.UI
{
    /// <summary>
    /// Селектор выбора дропдаун-списка клиента из записи аренды
    /// </summary>
    public class ReservationClientDropdownSelector : ReservationDropdownSelector
    {
        protected override string GetSelected(ReservationSingleSelectionContext reservationSingleSelectionContext) 
            => reservationSingleSelectionContext.GetSelectedData().ClientId;
    }
}