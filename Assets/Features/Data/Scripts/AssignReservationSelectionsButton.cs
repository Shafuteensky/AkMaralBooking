using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace StarletBooking.Data
{
    /// <summary>
    /// Кнопка назначения контекста данных записи аренды (+ дома и клиента из записи аренды)
    /// </summary>
    public class AssignReservationSelectionsButton : AssignSelectionContextButton
    {
        [FormerlySerializedAs("houseSelectionContext")]
        [Header("Контейнера данных записи аренды для вывода"), Space]
        [SerializeField] protected HouseSingleSelectionContext houseSingleSelectionContext;
        [FormerlySerializedAs("clientSelectionContext")] [SerializeField] protected ClientSingleSelectionContext clientSingleSelectionContext;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            if (selectionContext is not ReservationSingleSelectionContext reservationSelectionContext) return;
            
            houseSingleSelectionContext.Select(reservationSelectionContext.GetSelectedData().HouseId);
            clientSingleSelectionContext.Select(reservationSelectionContext.GetSelectedData().ClientId);
        }
    }
}