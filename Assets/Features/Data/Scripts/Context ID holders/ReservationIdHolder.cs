using Extensions.Data.InMemoryData.SelectionContext;
using StarletBooking.Data;

namespace ContextIdHolders
{
    /// <summary>
    /// Хранилище идентификатора записи брони
    /// </summary>
    public class ReservationIdHolder : ContextIdHolder<ReservationsDataContainer, ReservationData> { }
}