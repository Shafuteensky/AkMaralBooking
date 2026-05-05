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
            if (!TryGetReservationById(id, out ReservationData reservation)) return null;

            if (housesDataContainer == null)
            {
                ServiceDebug.LogError("Контейнер данных домов не назначен, данные не получены");    
                return null;
            }

            if (string.IsNullOrEmpty(reservation.HouseId))
            {
                ServiceDebug.LogError($"У записи аренды «{id}» не назначен идентификатор дома");
                return null;
            }
            
            return housesDataContainer.GetById(reservation.HouseId);
        }
        
        /// <summary>
        /// Клиент записи
        /// </summary>
        public ClientData GetClientById(string id)
        {
            if (!TryGetReservationById(id, out ReservationData reservation)) return null;

            if (clientsDataContainer == null)
            {
                ServiceDebug.LogError("Контейнер данных клиентов не назначен, данные не получены");    
                return null;
            }

            if (string.IsNullOrEmpty(reservation.ClientId))
            {
                ServiceDebug.LogError($"У записи аренды «{id}» не назначен идентификатор клиента");
                return null;
            }

            return clientsDataContainer.GetById(reservation.ClientId);
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
    }
}