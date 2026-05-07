using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о клиенте
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ClientSingleSelectionContext), menuName = "StarletBooking/Data/" + nameof(ClientSingleSelectionContext))]
    public class ClientSingleSelectionContext : SingleSelectionContext<ClientData> { }
}