using Extensions.Log;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Сохранение записи о доме
    /// </summary>
    public class SaveHouseDataButton : SaveDataButton<HousesDataContainer, HouseSelectionContext>
    {
        [Header("Поля данных"), Space]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _numberInputField;
        [SerializeField] private ColorPicker.ColorPicker _colorPicker;
        [SerializeField] private TMP_InputField _notesInputField;
        [SerializeField] private TMP_InputField paymentInputField;
        [SerializeField] private TMP_InputField _ownerNameInputField;
        [SerializeField] private TMP_InputField _ownerContactNumberInputField;
        
        public override void OnButtonClickAction()
        {
            if (dataContainer == null 
                || selectionContext == null)
            {
                ServiceDebug.LogError("Отсутствует ссылка на контейнер данных, запись не добавлена");
                return;
            }
            
            if (_nameInputField == null 
                || _numberInputField == null
                || _colorPicker == null
                || _notesInputField == null
                || paymentInputField == null
                || _ownerNameInputField == null 
                || _ownerContactNumberInputField == null)
            {
                ServiceDebug.LogError("Не все ссылки на поля ввода заполнены, запись не добавлена");
                return;
            }

            if (!selectionContext.HasSelection)
            {
                HouseData newHouse = new HouseData(
                    _nameInputField.text, _numberInputField.text, _colorPicker.GetColor(), 
                    EmptyIfDefault(_notesInputField.text), paymentInputField.text,
                    _ownerNameInputField.text, _ownerContactNumberInputField.text);
                dataContainer.Add(newHouse);
            }
            else
            {
                HouseData house = selectionContext.GetSelectedData();
                house.UpdateData(
                    _nameInputField.text, _numberInputField.text, _colorPicker.GetColor(), 
                    EmptyIfDefault(_notesInputField.text), paymentInputField.text,
                    _ownerNameInputField.text, _ownerContactNumberInputField.text);
                dataContainer.NotifyUpdated(house);
            }
        }
    }
}