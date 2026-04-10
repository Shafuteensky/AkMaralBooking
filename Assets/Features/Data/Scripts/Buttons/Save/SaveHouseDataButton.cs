using Extensions.Generics;
using Extensions.Log;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Сохранение записи о доме
    /// </summary>
    public class SaveHouseDataButton : AbstractButton
    {
        [SerializeField]
        private HousesDataContainer _houseDataContainer;
        [SerializeField]
        private TMP_InputField _nameInputField;
        [SerializeField]
        private TMP_InputField _numberInputField;
        [SerializeField]
        private TMP_InputField _notesInputField;
        [SerializeField]
        private TMP_InputField _ownerNameInputField;
        [SerializeField]
        private TMP_InputField _ownerContactNumberInputField;
        
        public override void OnButtonClick()
        {
            if (_houseDataContainer == null)
            {
                ServiceDebug.LogError("Отсутствует ссылка на контейнер данных, запись не добавлена");
                return;
            }
            
            if (_nameInputField == null 
                || _numberInputField == null
                || _notesInputField == null
                || _ownerNameInputField == null 
                || _ownerContactNumberInputField == null)
            {
                ServiceDebug.LogError("Не все ссылки на поля ввода заполнены, запись не добавлена");
                return;
            }
            
            HouseData newHouse = new HouseData(
                _nameInputField.text, _numberInputField.text, _notesInputField.text,
                _ownerNameInputField.text, _ownerContactNumberInputField.text);
            
            _houseDataContainer.Add(newHouse);
        }
    }
}