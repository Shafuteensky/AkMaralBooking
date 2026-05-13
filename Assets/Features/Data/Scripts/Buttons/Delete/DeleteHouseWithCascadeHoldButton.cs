using System.Collections.Generic;
using Extensions.Generics;
using Extensions.Helpers;
using UnityEngine;
using UnityEngine.UI;


namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Кнопка зажатия для удаления дома с опциональным удалением его записей аренды
    /// </summary>
    public sealed class DeleteHouseWithCascadeHoldButton : AbstractHoldButtonAction
    {
        [SerializeField] private HouseLinksChecker checker;
        [SerializeField] private ReservationsDataContainer reservationsContainer;
        [SerializeField] private Toggle deleteReservationsToggle;

        public override void OnButtonClickAction()
        {
            HouseSelectionContext context = DataBus.Instance.HouseSelectionContext;
            if (Logic.IsNull(context, nameof(context)) || !context.HasSelection) return;

            string houseId = context.SelectedId;

            if (ShouldDeleteReservations())
            {
                DeleteLinkedReservations();
                DataBus.Instance.ReservationSelectionContext?.Clear();
            }

            context.Container.Remove(houseId);
            context.Clear();
        }

        #region Internal

        private bool ShouldDeleteReservations() =>
            !Logic.IsNull(checker, nameof(checker)) &&
            deleteReservationsToggle != null &&
            deleteReservationsToggle.isOn &&
            !Logic.IsNull(reservationsContainer, nameof(reservationsContainer));

        private void DeleteLinkedReservations()
        {
            foreach (ReservationData reservation in new List<ReservationData>(checker.LinkedReservations))
            {
                if (reservation == null) continue;
                reservationsContainer.Remove(reservation);
            }
        }

        #endregion
    }
}
