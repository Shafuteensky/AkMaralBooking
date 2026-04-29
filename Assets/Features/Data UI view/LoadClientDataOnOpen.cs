using Extensions.Generics;
using Extensions.Helpers;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Заполнение UI данными клиента из SelectionContext при открытии страницы
    /// </summary>
    public class LoadClientDataOnOpen : SelectionContextViewLoader<ClientData>
    {
        [Header("Заполняемые поля"), Space]
        [SerializeField] protected TMP_InputField nameInputField;
        [SerializeField] protected TMP_InputField contactNumberInputField;
        [SerializeField] protected TMP_InputField notesInputField;
        [SerializeField] protected ColorPicker.ColorPicker colorPicker;
        [SerializeField] protected ToggleGroupControl ratingToggleGroup;

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
            
            colorPicker.SetColor(dataItem?.Color ?? DataHelpers.NotFoundColor);

            nameInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.Name);
            contactNumberInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.ContactNumber);
            notesInputField.text = dataItem == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(dataItem.Notes); 

            ratingToggleGroup.SetMode(ToggleGroupMode.Range);
            ratingToggleGroup.SetRange(dataItem?.Rating ?? 0);
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