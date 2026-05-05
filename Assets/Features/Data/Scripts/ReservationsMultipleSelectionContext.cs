using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст множественного выбора записей аренды
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ReservationsMultipleSelectionContext),
        menuName = "StarletBooking/Data/" + nameof(ReservationsMultipleSelectionContext)
    )]
    public sealed class ReservationsMultipleSelectionContext : MultipleSelectionContext<ReservationData> { }
}