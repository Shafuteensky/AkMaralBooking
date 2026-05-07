using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о доме
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HouseSelectionContext), menuName = "StarletBooking/Data/" + nameof(HouseSelectionContext))]
    public class HouseSelectionContext : SingleSelectionContext<HouseData> { }
}