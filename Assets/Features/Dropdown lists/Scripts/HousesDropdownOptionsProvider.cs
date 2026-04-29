using System.Collections.Generic;
using Extensions.Data.InMemoryData;
using Extensions.Helpers;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.UI
{
    [CreateAssetMenu(
        fileName = nameof(HousesDropdownOptionsProvider), 
        menuName = "StarletBooking/UI/Dropdown/" + nameof(HousesDropdownOptionsProvider))]
    public class HousesDropdownOptionsProvider : InMemoryDropdownOptionsProvider
    {
        [SerializeField]
        protected InMemoryDataContainer<HouseData> housesContainer = default;

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

            if (Logic.IsNull(housesContainer))
            {
                return;
            }

            IReadOnlyList<HouseData> data = housesContainer.Data;
            for (int i = 0; i < data.Count; i++)
            {
                HouseData house = data[i];
                if (house == null || string.IsNullOrEmpty(house.Id))
                {
                    continue;
                }

                string label = house.Name;
                if (!string.IsNullOrEmpty(house.Number))
                {
                    label += " (" + house.Number + ")";
                }

                labels.Add(label);
                ids.Add(house.Id);
            }
        }
    }
}