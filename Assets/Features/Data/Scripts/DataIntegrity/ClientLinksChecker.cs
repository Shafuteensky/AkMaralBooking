using System.Collections.Generic;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Проверяет связи клиента: записи аренды, привязанные к нему
    /// </summary>
    public sealed class ClientLinksChecker : DataLinksChecker<ClientData>
    {
        /// <summary>Записи аренды этого клиента</summary>
        public List<ReservationData> LinkedReservations { get; private set; } = new();

        /// <summary>Есть ли привязанные записи аренды</summary>
        public override bool HasLinks => LinkedReservations.Count > 0;

        [SerializeField] private ReservationsDataContainer reservationsContainer;

        protected override void PerformCheck(ClientData data)
        {
            LinkedReservations.Clear();

            if (Logic.IsNull(reservationsContainer, nameof(reservationsContainer))) return;
            if (string.IsNullOrEmpty(data.Id)) return;

            foreach (ReservationData reservation in reservationsContainer.Data)
            {
                if (reservation == null) continue;
                if (reservation.ClientId == data.Id)
                    LinkedReservations.Add(reservation);
            }
        }
    }
}
