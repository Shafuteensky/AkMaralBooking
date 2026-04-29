using Extensions.Log;
using StarletBooking.Data.View;
using StarletBooking.UI;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Загружает значения dropdown для клиента и дома
    /// </summary>
    public sealed class LoadReservationDropdowns : SelectionContextViewLoader<ReservationData>
    {
        [Header("Заполняемые списки"), Space]
        [SerializeField] private DropdownIdBinder clientsDropdown;
        [SerializeField] private DropdownIdBinder housesDropdown;

        protected override void ApplyToView(ReservationData dataItem)
        {
            if (clientsDropdown == null || housesDropdown == null)
            {
                ServiceDebug.LogError("Dropdown ссылки не заполнены");
                return;
            }

            clientsDropdown.SetSelectedById(dataItem.ClientId);
            housesDropdown.SetSelectedById(dataItem.HouseId);
        }

        protected override void ApplyEmpty()
        {
            if (clientsDropdown != null) { clientsDropdown.ClearSelection(); }
            if (housesDropdown != null) { housesDropdown.ClearSelection(); }
        }
    }
}