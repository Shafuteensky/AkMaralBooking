using System.Collections.Generic;
using Extensions.Data.InMemoryData;
using Extensions.Helpers;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.UI
{
    [CreateAssetMenu(fileName = nameof(ClientsDropdownOptionsProvider), 
        menuName = "StarletBooking/UI/Dropdown/" + nameof(ClientsDropdownOptionsProvider))]
    public class ClientsDropdownOptionsProvider : InMemoryDropdownOptionsProvider
    {
        [SerializeField]
        protected InMemoryDataContainer<ClientData> clientsContainer = default;

        [SerializeField]
        protected bool includeEmptyOption = true;

        [SerializeField]
        protected string emptyLabel = DEFAULT_EMPTY_TEXT;

        [SerializeField]
        protected string emptyId = string.Empty;

        public override void BuildOptions(List<string> labels, List<string> ids)
        {
            labels.Clear();
            ids.Clear();

            if (includeEmptyOption)
            {
                labels.Add(emptyLabel);
                ids.Add(emptyId);
            }

            if (Logic.IsNull(clientsContainer))
            {
                return;
            }

            IReadOnlyList<ClientData> data = clientsContainer.Data;
            for (int i = 0; i < data.Count; i++)
            {
                ClientData client = data[i];
                if (client == null || string.IsNullOrEmpty(client.Id))
                {
                    continue;
                }

                labels.Add(client.Name);
                ids.Add(client.Id);
            }
        }
    }
}