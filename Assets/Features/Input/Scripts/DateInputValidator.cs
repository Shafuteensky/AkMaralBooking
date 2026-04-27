using System.Text;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Валидация и форматирование ввода даты в формате 00.00.00
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DateInputValidator),
        menuName = "StarletBooking/UI/Input/" + nameof(DateInputValidator))]
    public sealed class DateInputValidator : TMP_InputValidator
    {
        private const int MaxDigits = 6;

        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (!char.IsDigit(ch)) return '\0';

            int digitIndex = GetDigitsCountBeforePosition(text, pos);
            string digits = GetDigits(text);

            if (digits.Length < MaxDigits)
                digits = digits.Insert(digitIndex, ch.ToString());
            else if (digitIndex < MaxDigits)
            {
                StringBuilder digitsBuilder = new StringBuilder(digits);
                digitsBuilder[digitIndex] = ch;
                digits = digitsBuilder.ToString();
            }
            else
                return '\0';

            text = Format(digits);
            pos = GetPositionByDigitsCount(text, digitIndex + 1);

            return ch;
        }

        private string GetDigits(string input)
        {
            StringBuilder sb = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i])) sb.Append(input[i]);
            }

            return sb.ToString();
        }

        private int GetDigitsCountBeforePosition(string input, int position)
        {
            int count = 0;
            int max = Mathf.Min(position, input.Length);

            for (int i = 0; i < max; i++)
            {
                if (char.IsDigit(input[i])) count++;
            }

            return count;
        }

        private int GetPositionByDigitsCount(string input, int digitsCount)
        {
            if (digitsCount <= 0) return 0;

            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    count++;
                    if (count == digitsCount) return i + 1;
                }
            }

            return input.Length;
        }

        private string Format(string digits)
        {
            if (digits.Length <= 2) return digits;
            if (digits.Length <= 4) return digits.Insert(2, ".");
            return digits.Insert(2, ".").Insert(5, ".");
        }
    }
}