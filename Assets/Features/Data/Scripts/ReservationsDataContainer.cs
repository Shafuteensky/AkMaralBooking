using Extensions.Data.InMemoryData;
using Extensions.Log;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контейнер данных записей аренды
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ReservationsDataContainer),
        menuName = "StarletBooking/Data/" + nameof(ReservationsDataContainer))]
    public class ReservationsDataContainer : InMemoryDataContainer<ReservationData>
    {
        [Header("Связанные контейнера"), Space]
        [SerializeField] private HousesDataContainer housesDataContainer;
        [SerializeField] private ClientsDataContainer clientsDataContainer;

        /// <summary>
        /// Дом записи
        /// </summary>
        public HouseData GetHouseById(string id)
        {
            if (!TryGetReservationById(id, out ReservationData reservation) ||
                !IsContainerAssigned(housesDataContainer, "Контейнер данных домов"))
            {
                return null;
            }

            if (string.IsNullOrEmpty(reservation.HouseId))
            {
                ServiceDebug.LogError($"У записи аренды «{id}» не назначен идентификатор дома");
                return null;
            }

            if (!housesDataContainer.GetById(reservation.HouseId, out HouseData house))
            {
                ServiceDebug.LogError($"Дом с идентификатором «{reservation.HouseId}» не найден");
                return null;
            }

            return house;
        }

        /// <summary>
        /// Клиент записи
        /// </summary>
        public ClientData GetClientById(string id)
        {
            if (!TryGetReservationById(id, out ReservationData reservation) || 
                !IsContainerAssigned(clientsDataContainer, "Контейнер данных клиентов"))
            {
                return null;
            }

            if (string.IsNullOrEmpty(reservation.ClientId))
            {
                ServiceDebug.LogError($"У записи аренды «{id}» не назначен идентификатор клиента");
                return null;
            }

            if (!clientsDataContainer.GetById(reservation.ClientId, out ClientData client))
            {
                ServiceDebug.LogError($"Клиент с идентификатором «{reservation.ClientId}» не найден");
                return null;
            }

            return client;
        }

        private bool TryGetReservationById(string id, out ReservationData reservation)
        {
            reservation = null;

            if (string.IsNullOrEmpty(id))
            {
                ServiceDebug.LogError("Передан пустой идентификатор записи аренды");
                return false;
            }

            if (!GetById(id, out reservation))
            {
                ServiceDebug.LogError($"Запись аренды с идентификатором «{id}» не найдена");
                return false;
            }

            return true;
        }

        private bool IsContainerAssigned(InMemoryDataBaseObject container, string containerName)
        {
            if (container != null) return true;

            ServiceDebug.LogError($"{containerName} не назначен, данные не получены");
            return false;
        }
    }
}