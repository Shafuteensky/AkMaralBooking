using Extensions.Log;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Селектор выбора дропдаун-списка из записи аренды
    /// </summary>
    public abstract class ReservationDropdownSelector : DropdownOptionSelector
    {
        protected abstract string GetSelected(ReservationSelectionContext reservationSelectionContext);
        
        protected override void Select()
        {
            if (selectionContext is not ReservationSelectionContext reservationSelectionContext)
            {
                ServiceDebug.LogError("Неверный тип контекста выбора, должен быть " + nameof(ReservationSelectionContext));
                return;
            }
            
            if (!reservationSelectionContext.HasSelection)
            {
                binder.ClearSelection();
                return;
            }

            string selected = GetSelected(reservationSelectionContext);
            
            if (!string.IsNullOrEmpty(selected)) binder.SetSelectedById(selected);
        }
    }
}