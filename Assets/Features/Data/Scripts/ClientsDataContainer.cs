using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контейнер данных о клиентах
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ClientsDataContainer), menuName = "StarletBooking/Data/" + nameof(ClientsDataContainer))]
    public class ClientsDataContainer : InMemoryDataContainer<ClientData> { }
}