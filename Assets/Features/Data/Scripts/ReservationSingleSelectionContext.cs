using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о записи аренды
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ReservationSingleSelectionContext), 
        menuName = "StarletBooking/Data/" + nameof(ReservationSingleSelectionContext))]
    public class ReservationSingleSelectionContext : SingleSelectionContext<ReservationData> { }
}