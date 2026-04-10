using StarletBooking.Data;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Заполнение фабрикой списка записей аренды по завершению сохранения
    /// </summary>
    public class PopulateReservationsOnAfterSave : PopulateOnAfterSave<ReservationsDataContainer, ReservationData> { }
}