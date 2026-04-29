using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Валидация ввода номера телефона
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(PhoneInputValidator),
        menuName = "StarletBooking/UI/Input/" + nameof(PhoneInputValidator))]
    public class PhoneInputValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (char.IsDigit(ch))
            {
                text = text.Insert(pos, ch.ToString());
                pos++;
                return ch;
            }

            if (ch == '+')
            {
                if (pos == 0 && !text.Contains("+"))
                {
                    text = text.Insert(pos, ch.ToString());
                    pos++;
                    return ch;
                }

                return '\0';
            }

            if (ch == ' ' || ch == '-' || ch == '(' || ch == ')')
            {
                text = text.Insert(pos, ch.ToString());
                pos++;
                return ch;
            }

            return '\0';
        }
    }
}