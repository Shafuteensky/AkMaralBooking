using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StarletBooking.Data
{
    /// <summary>
    /// Кнопка назначения контекста данных записи аренды (+ дома и клиента из записи аренды)
    /// </summary>
    public class AssignReservationSelectionsButton : AssignSelectionContextButton
    {
        [Header("Контейнера данных записи аренды для вывода"), Space]
        [SerializeField] protected HouseSelectionContext houseSelectionContext;
        [SerializeField] protected ClientSelectionContext clientSelectionContext;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            if (selectionContext is not ReservationSelectionContext reservationSelectionContext) return;
            
            houseSelectionContext.Select(reservationSelectionContext.GetSelectedData().HouseId);
            clientSelectionContext.Select(reservationSelectionContext.GetSelectedData().ClientId);
        }
    }
}