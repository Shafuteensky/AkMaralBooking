using StarletBooking.Data;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Очищает поле при фокусе, если текст равен DataHelpers.EmptyString
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public sealed class ClearOnFocusIfEmptyString : MonoBehaviour
    {
        private TMP_InputField inputField;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        private void OnEnable()
        {
            inputField.onSelect.AddListener(OnSelect);
        }

        private void OnDisable()
        {
            inputField.onSelect.RemoveListener(OnSelect);
        }

        private void OnSelect(string _)
        {
            if (inputField.text == DataHelpers.EmptyString) inputField.text = string.Empty;
        }
    }
}