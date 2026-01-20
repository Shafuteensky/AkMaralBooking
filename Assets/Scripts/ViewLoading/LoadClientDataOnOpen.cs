using Extensions.Generics;
using Extensions.Logic;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Заполнение UI данными клиента из SelectionContext при открытии страницы
    /// </summary>
    public class LoadClientDataOnOpen : SelectionContextViewLoader<ClientData>
    {
        [SerializeField]
        protected TMP_InputField nameInputField = default;
        [SerializeField]
        protected TMP_InputField contactNumberInputField = default;
        [SerializeField] 
        protected ColorPicker.ColorPicker colorPicker;
        [SerializeField]
        protected GenericToggleGroup ratingToggleGroup = default;
        [SerializeField]
        protected TMP_InputField notesInputField = default;

        protected override void ApplyToView(ClientData dataItem)
        {
            if (Logic.IsNull(colorPicker) ||
                Logic.IsNull(nameInputField) ||
                Logic.IsNull(contactNumberInputField) ||
                Logic.IsNull(ratingToggleGroup) ||
                Logic.IsNull(notesInputField))
            {
                return;
            }
            
            colorPicker.SetColor(dataItem.Color);
            nameInputField.text = dataItem.Name;
            contactNumberInputField.text = dataItem.ContactNumber;
            notesInputField.text = dataItem.Notes;

            ratingToggleGroup.SetMode(ToggleGroupMode.Range);
            ratingToggleGroup.SetRange(dataItem.Rating);
        }

        protected override void ApplyEmpty()
        {
            if (nameInputField != null) { nameInputField.text = string.Empty; }
            if (colorPicker != null) { colorPicker.SetColor(Random.ColorHSV()); }
            if (contactNumberInputField != null) { contactNumberInputField.text = string.Empty; }
            if (notesInputField != null) { notesInputField.text = string.Empty; }

            if (ratingToggleGroup != null)
            {
                ratingToggleGroup.SetMode(ToggleGroupMode.Range);
                ratingToggleGroup.SetRange(0);
            }
        }
    }
}