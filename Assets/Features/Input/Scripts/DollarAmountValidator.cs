using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Валидация суммы в долларах (формат: 123$)
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DollarAmountValidator),
        menuName = "StarletBooking/UI/Input/" + nameof(DollarAmountValidator))]
    public class DollarAmountValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "0$";
                pos = 1;
            }

            string digits = text.Replace("$", "");

            if (char.IsDigit(ch))
            {
                digits = digits.Insert(pos, ch.ToString());
                text = Normalize(digits);
                pos = text.Length - 1;
                return ch;
            }

            return '\0';
        }

        private string Normalize(string digits)
        {
            if (string.IsNullOrEmpty(digits))
            {
                digits = "0";
            }

            // убираем ведущие нули
            digits = digits.TrimStart('0');

            if (string.IsNullOrEmpty(digits))
            {
                digits = "0";
            }

            return digits + "$";
        }
    }
}