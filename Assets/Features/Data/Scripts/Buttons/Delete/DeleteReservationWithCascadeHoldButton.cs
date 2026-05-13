using System.Collections.Generic;
using Extensions.Generics;
using Extensions.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Кнопка зажатия для удаления записи аренды с опциональным удалением клиента
    /// и других его записей аренды
    /// </summary>
    public sealed class DeleteReservationWithCascadeHoldButton : AbstractHoldButtonAction
    {
        [SerializeField] private ReservationLinksChecker checker;
        [SerializeField] private ClientsDataContainer clientsContainer;
        [SerializeField] private Toggle deleteClientToggle;
        [SerializeField] private Toggle deleteOtherReservationsToggle;

        public override void OnButtonClickAction()
        {
            ReservationSelectionContext context = DataBus.Instance.ReservationSelectionContext;
            if (Logic.IsNull(context, nameof(context)) || !context.HasSelection) return;

            string reservationId = context.SelectedId;

            if (ShouldDeleteOtherReservations())
                DeleteOtherClientReservations(context);

            if (ShouldDeleteClient())
                DeleteLinkedClient();

            context.Container.Remove(reservationId);
            context.Clear();
        }

        #region Internal

        private bool ShouldDeleteOtherReservations() =>
            !Logic.IsNull(checker, nameof(checker)) &&
            deleteOtherReservationsToggle != null &&
            deleteOtherReservationsToggle.isOn;

        private bool ShouldDeleteClient() =>
            !Logic.IsNull(checker, nameof(checker)) &&
            deleteClientToggle != null &&
            deleteClientToggle.isOn &&
            checker.HasLinkedClient &&
            !Logic.IsNull(clientsContainer, nameof(clientsContainer));

        private void DeleteOtherClientReservations(ReservationSelectionContext context)
        {
            foreach (ReservationData reservation in new List<ReservationData>(checker.OtherClientReservations))
            {
                if (reservation == null) continue;
                context.Container.Remove(reservation);
            }
        }

        private void DeleteLinkedClient()
        {
            clientsContainer.Remove(checker.LinkedClient);
            DataBus.Instance.ClientSelectionContext.Clear();
        }

        #endregion
    }
}
