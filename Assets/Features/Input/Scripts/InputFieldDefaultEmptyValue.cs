using TMPro;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Поддерживает нулевое значение в TMP_InputField:
    /// очищает "0" при начале ввода и возвращает "0", если поле осталось пустым
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public sealed class InputFieldDefaultEmptyValue : MonoBehaviour
    {
        [SerializeField] private string DefaultValue = "0";

        private TMP_InputField inputField;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        private void OnEnable()
        {
            inputField.onSelect.AddListener(OnSelect);
            inputField.onEndEdit.AddListener(OnEndEdit);

            EnsureNotEmpty();
        }

        private void OnDisable()
        {
            inputField.onSelect.RemoveListener(OnSelect);
            inputField.onEndEdit.RemoveListener(OnEndEdit);
        }

        private void OnSelect(string value)
        {
            if (value == DefaultValue)
            {
                inputField.text = string.Empty;
            }
        }

        private void OnEndEdit(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                inputField.text = DefaultValue;
            }
        }

        private void EnsureNotEmpty()
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = DefaultValue;
            }
        }
    }
}