using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Валидация количества дней (минимум 1)
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DaysAmountValidator),
        menuName = "StarletBooking/UI/Input/" + nameof(DaysAmountValidator))]
    public class DaysAmountValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (!char.IsDigit(ch))
            {
                return '\0';
            }

            string newText = text.Insert(pos, ch.ToString());

            newText = newText.TrimStart('0');

            if (string.IsNullOrEmpty(newText))
            {
                newText = "1";
            }

            int value = int.Parse(newText);

            if (value < 1)
            {
                newText = "1";
            }

            text = newText;
            pos = text.Length;

            return ch;
        }
    }
}