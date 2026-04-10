using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о записи аренды
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ReservationSelectionContext), menuName = "StarletBooking/Data/" + nameof(ReservationSelectionContext))]
    public class ReservationSelectionContext : SelectionContext<ReservationData> { }
}