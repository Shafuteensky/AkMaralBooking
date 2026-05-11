using System;
using System.Collections.Generic;
using StarletBooking.Data;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Фабрика списка клиентов
    /// </summary>
    public class ClientPreviewButtonFactory : GenericDataPreviewButtonFactory<ClientsDataContainer, ClientData>
    {
        /// <summary>
        /// Сортировка клиентов по ФИО (в алфавитном порядке), затем по контактному номеру
        /// </summary>
        protected override void SortData(List<ClientData> data)
        {
            data.Sort(CompareClients);
        }

        private int CompareClients(ClientData first, ClientData second)
        {
            if (first == null && second == null) return 0;
            if (first == null) return 1;
            if (second == null) return -1;

            int nameComparison = string.Compare(first.Name, second.Name, StringComparison.CurrentCultureIgnoreCase);
            if (nameComparison != 0) return nameComparison;

            return string.Compare(first.ContactNumber, second.ContactNumber, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}