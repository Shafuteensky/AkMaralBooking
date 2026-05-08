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
        private HouseSelectionContext houseSelectionContext;
        private ClientSelectionContext clientSelectionContext;

        protected override void Awake()
        {
            base.Awake();
            houseSelectionContext = DataBus.Instance.HouseSelectionContext;
            clientSelectionContext = DataBus.Instance.ClientSelectionContext;
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            if (selectionContext is not ReservationSelectionContext reservationSelectionContext) return;
            
            houseSelectionContext.Select(reservationSelectionContext.GetSelectedData().HouseId);
            clientSelectionContext.Select(reservationSelectionContext.GetSelectedData().ClientId);
        }
    }
}