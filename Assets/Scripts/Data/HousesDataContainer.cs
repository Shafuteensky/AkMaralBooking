using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контейнер данных о домах
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HousesDataContainer), menuName = "StarletBooking/Data/" + nameof(HousesDataContainer))]
    public class HousesDataContainer : InMemoryDataContainer<HouseData> { }
}