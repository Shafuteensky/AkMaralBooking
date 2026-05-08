using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Проверяет ввод десятичного числа и заменяет десятичный разделитель на точку
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DecimalNumberValidator),
        menuName = "StarletBooking/UI/Input/" + nameof(DecimalNumberValidator))]
    public sealed class DecimalNumberValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (char.IsDigit(ch))
            {
                text = text.Insert(pos, ch.ToString());
                pos++;
                return ch;
            }

            if (ch == '.' || ch == ',')
            {
                if (text.Contains(".") || text.Contains(","))
                {
                    return '\0';
                }

                if (text.Length == 0)
                {
                    text = "0.";
                    pos = text.Length;
                    return '.';
                }

                text = text.Insert(pos, ".");
                pos++;
                return '.';
            }

            return '\0';
        }
    }
}