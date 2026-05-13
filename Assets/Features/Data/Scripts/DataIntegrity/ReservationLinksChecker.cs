using System.Collections.Generic;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Проверяет связи записи аренды: привязанный клиент и его другие записи аренды
    /// </summary>
    public sealed class ReservationLinksChecker : DataLinksChecker<ReservationData>
    {
        /// <summary>Клиент, привязанный к записи аренды</summary>
        public ClientData LinkedClient { get; private set; }

        /// <summary>Другие записи аренды того же клиента</summary>
        public List<ReservationData> OtherClientReservations { get; private set; } = new();

        /// <summary>Есть ли привязанный клиент</summary>
        public bool HasLinkedClient => LinkedClient != null;

        /// <summary>Есть ли другие записи аренды этого клиента</summary>
        public bool HasOtherClientReservations => OtherClientReservations.Count > 0;

        /// <summary>Есть ли хоть какие-либо связанные записи</summary>
        public override bool HasLinks => HasLinkedClient || HasOtherClientReservations;

        [SerializeField] private ClientsDataContainer clientsContainer;
        [SerializeField] private ReservationsDataContainer reservationsContainer;

        protected override void PerformCheck(ReservationData data)
        {
            LinkedClient = null;
            OtherClientReservations.Clear();

            if (!string.IsNullOrEmpty(data.ClientId) && clientsContainer != null)
            {
                clientsContainer.GetById(data.ClientId, out ClientData client);
                LinkedClient = client;
            }

            if (Logic.IsNull(reservationsContainer, nameof(reservationsContainer))) return;

            foreach (ReservationData reservation in reservationsContainer.Data)
            {
                if (reservation == null || reservation.Id == data.Id) continue;
                if (reservation.ClientId == data.ClientId)
                    OtherClientReservations.Add(reservation);
            }
        }
    }
}
