using Extensions.Data.InMemoryData.SelectionContext;
using StarletBooking.Data;

namespace ContextIdHolders
{
    /// <summary>
    /// Хранилище идентификатора клиента
    /// </summary>
    public class ClientIdHolder : ContextIdHolder<ClientsDataContainer, ClientData> { }
}