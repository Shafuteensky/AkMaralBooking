using Extensions.Generics;
using Extensions.Log;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Сохранение записи о клиенте
    /// </summary>
    public class SaveClientDataButton : GenericButton
    {
        [SerializeField]
        private ClientsDataContainer _clientsDataContainer;
        [SerializeField]
        private TMP_InputField _nameInputField;
        [SerializeField] 
        private ColorPicker.ColorPicker _colorPicker;
        [SerializeField]
        private TMP_InputField _contactNumberInputField;
        [SerializeField]
        private GenericToggleGroup _ratingToggleGroup;
        [SerializeField]
        private TMP_InputField _notesInputField;
        
        public override void OnButtonClick()
        {
            if (_clientsDataContainer == null)
            {
                ServiceDebug.LogError("Отсутствует ссылка на контейнер данных, запись не добавлена");
                return;
            }
            
            if (_colorPicker == null
                || _nameInputField == null 
                || _contactNumberInputField == null
                || _ratingToggleGroup == null 
                || _notesInputField == null)
            {
                ServiceDebug.LogError("Не все ссылки на поля ввода заполнены, запись не добавлена");
                return;
            }
            
            ClientData newClient = new ClientData(
                _nameInputField.text, _colorPicker.GetColor(), _contactNumberInputField.text, 
                _ratingToggleGroup.GetActiveCount(), _notesInputField.text);
            
            _clientsDataContainer.Add(newClient);
        }
    }
}