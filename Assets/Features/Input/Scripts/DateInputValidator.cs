using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Валидация ввода даты (только цифры)
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DateInputValidator),
        menuName = "StarletBooking/UI/Input/" + nameof(DateInputValidator))]
    public class DateInputValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (!char.IsDigit(ch))
            {
                return '\0';
            }

            text = text.Insert(pos, ch.ToString());
            pos++;

            return ch;
        }
    }
}