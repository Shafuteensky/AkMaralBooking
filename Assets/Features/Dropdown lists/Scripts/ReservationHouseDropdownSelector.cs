using StarletBooking.Data;

namespace StarletBooking.UI
{
    /// <summary>
    /// Селектор выбора дропдаун-списка домов из записи аренды
    /// </summary>
    public class ReservationHouseDropdownSelector : ReservationDropdownSelector
    {
        protected override string GetSelected(ReservationSingleSelectionContext reservationSingleSelectionContext) 
            => reservationSingleSelectionContext.GetSelectedData().HouseId;
    }
}