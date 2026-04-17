using System.Text;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Форматирование вводимого текста (дата дд.мм.гг)
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class DateInputFormatter : MonoBehaviour
    {
        private const int MaxDigits = 6;
        private TMP_InputField inputField;
        private bool _internalUpdate;

        private void Awake()
        {
            if (inputField == null)
            {
                inputField = GetComponent<TMP_InputField>();
            }
        }

        private void OnEnable()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);
            ApplyFormatting(inputField.text, true);
        }

        private void OnDisable()
        {
            inputField.onValueChanged.RemoveListener(OnValueChanged);
        }

        /// <summary>
        /// Проверка валидности даты
        /// </summary>
        public bool IsValid()
        {
            string raw = GetDigits(inputField.text);

            if (raw.Length != 6)
            {
                return false;
            }

            int day = int.Parse(raw.Substring(0, 2));
            int month = int.Parse(raw.Substring(2, 2));
            int year = int.Parse(raw.Substring(4, 2));

            year += 2000;

            if (month < 1 || month > 12)
            {
                return false;
            }

            int daysInMonth = System.DateTime.DaysInMonth(year, month);

            if (day < 1 || day > daysInMonth)
            {
                return false;
            }

            return true;
        }

        private void OnValueChanged(string value)
        {
            if (_internalUpdate)
            {
                return;
            }

            ApplyFormatting(value, false);
        }

        private void ApplyFormatting(string value, bool moveCaretToEnd)
        {
            string digits = GetDigits(value);

            if (digits.Length > MaxDigits)
            {
                digits = digits.Substring(0, MaxDigits);
            }

            string formatted = Format(digits);

            _internalUpdate = true;
            inputField.SetTextWithoutNotify(formatted);

            int caret = moveCaretToEnd ? formatted.Length : inputField.text.Length;
            inputField.caretPosition = caret;

            _internalUpdate = false;
        }

        private string GetDigits(string input)
        {
            StringBuilder sb = new StringBuilder(input.Length);

            foreach (var t in input)
            {
                if (char.IsDigit(t))
                {
                    sb.Append(t);
                }
            }

            return sb.ToString();
        }

        private string Format(string digits)
        {
            if (digits.Length <= 2)
            {
                return digits;
            }

            if (digits.Length <= 4)
            {
                return digits.Insert(2, ".");
            }

            return digits.Insert(2, ".").Insert(5, ".");
        }
    }
}