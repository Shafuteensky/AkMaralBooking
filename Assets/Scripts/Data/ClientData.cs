using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Данные о клиенте
    /// </summary>
    public class ClientData : InMemoryDataItem
    {
        [field: SerializeField]
        public string Name { get; private set; }
    }
}