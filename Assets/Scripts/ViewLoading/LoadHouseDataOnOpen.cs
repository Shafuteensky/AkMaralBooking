using Extensions.Logic;
using StarletBooking.Data.View;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Заполнение UI данными дома из SelectionContext при открытии страницы
    /// </summary>
    public class LoadHouseDataOnOpen : SelectionContextViewLoader<HouseData>
    {
        [SerializeField]
        protected TMP_InputField nameInputField = default;
        [SerializeField]
        protected TMP_InputField numberInputField = default;
        [SerializeField]
        protected TMP_InputField notesInputField = default;
        [SerializeField]
        protected TMP_InputField ownerNameInputField = default;
        [SerializeField]
        protected TMP_InputField ownerContactNumberInputField = default;

        protected override void ApplyToView(HouseData dataItem)
        {
            if (Logic.IsNull(nameInputField) ||
                Logic.IsNull(numberInputField) ||
                Logic.IsNull(notesInputField) ||
                Logic.IsNull(ownerNameInputField) ||
                Logic.IsNull(ownerContactNumberInputField))
            {
                return;
            }

            nameInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.Name); 
            numberInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.Number);
            notesInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.Notes); 
            ownerNameInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.OwnerName); 
            ownerContactNumberInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.OwnerContactNumber);
        }

        protected override void ApplyEmpty()
        {
            if (nameInputField != null) { nameInputField.text = string.Empty; }
            if (numberInputField != null) { numberInputField.text = string.Empty; }
            if (notesInputField != null) { notesInputField.text = string.Empty; }
            if (ownerNameInputField != null) { ownerNameInputField.text = string.Empty; }
            if (ownerContactNumberInputField != null) { ownerContactNumberInputField.text = string.Empty; }
        }
    }
}