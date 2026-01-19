using Extensions.Generics;
using Extensions.Logic;
using StarletBooking.Data.View;
using StarletBooking.UI;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Заполнение UI данными записи аренды из SelectionContext при открытии страницы
    /// </summary>
    public class LoadReservationDataOnOpen : SelectionContextViewLoader<ReservationData>
    {
        [SerializeField]
        protected TMP_InputField paymentPerDayInputField = default;
        [SerializeField]
        protected TMP_InputField daysInputField = default;
        [SerializeField]
        protected TMP_InputField prepaymentInputField = default;
        [SerializeField]
        protected TMP_InputField exchangeRateInputField = default;

        [Header("Edit")]
        [SerializeField]
        protected DropdownIdBinder clientsDropdown = default;
        
        [Header("View")]
        [SerializeField]
        protected ClientsDataContainer clientsContainer;
        [SerializeField]
        protected TMP_InputField clientNameInputField = default;
        [SerializeField]
        protected TMP_InputField clientNumberInputField = default;
        [SerializeField]
        protected GenericToggleGroup clientRatingToggleGroup = default;
        [SerializeField]
        protected TMP_InputField clientNotesInputField = default;
        
        protected override void ApplyToView(ReservationData dataItem)
        {
            if (Logic.IsNull(paymentPerDayInputField) ||
                Logic.IsNull(daysInputField) ||
                Logic.IsNull(exchangeRateInputField) ||
                Logic.IsNull(prepaymentInputField))
            { return;}
            
            if (clientsDropdown == null &&
                Logic.IsNull(clientsContainer))
            { return;}

            paymentPerDayInputField.text = dataItem.PaymentPerDay.ToString();
            daysInputField.text = dataItem.Days.ToString();
            prepaymentInputField.text = dataItem.Prepayment.ToString();
            exchangeRateInputField.text = dataItem.ExchangeRate.ToString();

            if (clientsDropdown == null &&
                clientsContainer.TryGetById(dataItem.ClientId, out ClientData clientItem))
            {
                clientNameInputField.text = clientItem.Name;
                clientNumberInputField.text = clientItem.ContactNumber;
                clientNotesInputField.text = clientItem.Notes;

                clientRatingToggleGroup.SetMode(ToggleGroupMode.Range);
                clientRatingToggleGroup.SetRange(clientItem.Rating);
            }
            else
                clientsDropdown.SetSelectedById(dataItem.ClientId);
        }

        protected override void ApplyEmpty()
        {
            if (clientsDropdown != null)
            {
                clientsDropdown.SetSelectedById(string.Empty);
            }

            if (paymentPerDayInputField != null) { paymentPerDayInputField.text = string.Empty; }
            if (daysInputField != null) { daysInputField.text = string.Empty; }
            if (prepaymentInputField != null) { prepaymentInputField.text = string.Empty; }
            if (exchangeRateInputField != null) { exchangeRateInputField.text = string.Empty; }
        }
    }
}
