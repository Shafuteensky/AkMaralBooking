using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о доме
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HouseSingleSelectionContext), menuName = "StarletBooking/Data/" + nameof(HouseSingleSelectionContext))]
    public class HouseSingleSelectionContext : SingleSelectionContext<HouseData> { }
}