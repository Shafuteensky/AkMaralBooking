using Extensions.Generics;
using Extensions.Log;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Сохранение записи о клиенте
    /// </summary>
    public class SaveClientDataButton : SaveDataButton<ClientsDataContainer, ClientSingleSelectionContext>
    {
        [Header("Поля данных"), Space]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private ColorPicker.ColorPicker _colorPicker;
        [SerializeField] private TMP_InputField _contactNumberInputField;
        [SerializeField] private ToggleGroupControl _ratingToggleGroup;
        [SerializeField] private TMP_InputField _notesInputField;
        
        public override void OnButtonClickAction()
        {
            if (dataContainer == null
                || selectionContext == null)
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
            
            if (!selectionContext.HasSelection)
            {
                ClientData newClient = new ClientData(
                    _nameInputField.text, _colorPicker.GetColor(), _contactNumberInputField.text, 
                    _ratingToggleGroup.GetActiveCount(), _notesInputField.text);
                dataContainer.Add(newClient);
            }
            else
            {
                selectionContext.GetSelectedData().UpdateData(
                    _nameInputField.text, _colorPicker.GetColor(), _contactNumberInputField.text, 
                    _ratingToggleGroup.GetActiveCount(), _notesInputField.text);
                dataContainer.NotifyUpdated();
            }
        }
    }
}