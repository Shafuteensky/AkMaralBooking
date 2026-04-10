using Extensions.Data.InMemoryData.SelectionContext;
using StarletBooking.Data;

namespace ContextIdHolders
{
    /// <summary>
    /// Хранилище идентификатора дома
    /// </summary>
    public class HouseIdHolder : ContextIdHolder<HousesDataContainer, HouseData> { }
}