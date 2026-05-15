using Extensions.Generics;
using StarletBooking.Data;
using System.Text;
using TMPro;
using UnityEngine;

namespace StarletBooking.PhoneContact
{
    public enum PhoneContactType { Call, WhatsApp }

    /// <summary>
    /// Открывает телефонный звонок или WhatsApp для номера выбранного клиента.
    /// Interactable управляется через <see cref="PhoneContactAvailabilityBase"/>.
    /// </summary>
    public sealed class PhoneContactButton : AbstractButtonAction
    {
        [Header("Параметры"), Space]
        [SerializeField] private TMP_InputField phoneNumber;
        [SerializeField] private PhoneContactType type;

        /// <summary>Открыть звонок или чат WhatsApp по номеру выбранного клиента</summary>
        public override void OnButtonClickAction()
        {
            string number = phoneNumber.text;
            if (string.IsNullOrWhiteSpace(number)) return;

            string url = type == PhoneContactType.Call
                ? "tel:" + number
                : "https://wa.me/" + ExtractDigits(number);

            Application.OpenURL(url);
        }

        private static string ExtractDigits(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
                if (char.IsDigit(c)) sb.Append(c);
            return sb.ToString();
        }
    }
}
