using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контейнер данных записей аренды
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ReservationsDataContainer), menuName = "StarletBooking/Data/" + nameof(ReservationsDataContainer))]
    public class ReservationsDataContainer : InMemoryDataContainer<ReservationData> { }
}