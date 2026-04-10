using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о клиенте
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ClientSelectionContext), menuName = "StarletBooking/Data/" + nameof(ClientSelectionContext))]
    public class ClientSelectionContext : SelectionContext<ClientData> { }
}