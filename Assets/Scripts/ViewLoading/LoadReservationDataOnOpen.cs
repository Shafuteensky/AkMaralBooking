using Extensions.Generics;
using Extensions.Log;
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
        [SerializeField] 
        private TMP_InputField arrivalDate;
        [SerializeField] 
        private TMP_InputField departureDate;

        [Header("Edit")]
        [SerializeField]
        protected DropdownIdBinder clientsDropdown = default;
        [SerializeField]
        protected DropdownIdBinder housesDropdown = default;
        
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
        [SerializeField]
        protected HousesDataContainer housesContainer;
        [SerializeField]
        protected TMP_InputField houseNameInputField = default;
        [SerializeField]
        protected TMP_InputField houseNumberInputField = default;
        [SerializeField]
        protected TMP_InputField houseOwnerNameInputField = default;
        [SerializeField]
        protected TMP_InputField houseOwnerNumberInputField = default;
        [SerializeField]
        protected TMP_InputField houseNotesInputField = default;
        
        protected override void ApplyToView(ReservationData dataItem)
        {
            if ((clientsDropdown == null &&
                 clientsContainer == null) ||
                (housesDropdown == null &&
                 housesContainer == null))
            {
                ServiceDebug.LogError("Ссылки не заполнены");
                return;
            }
            
            if (Logic.IsNull(paymentPerDayInputField) ||
                Logic.IsNull(daysInputField) ||
                Logic.IsNull(arrivalDate) ||
                Logic.IsNull(departureDate) ||
                Logic.IsNull(exchangeRateInputField) ||
                Logic.IsNull(prepaymentInputField))
            { return;}

            paymentPerDayInputField.text = dataItem.PaymentPerDay.ToString();
            daysInputField.text = dataItem.Days.ToString();
            arrivalDate.text = dataItem.ArrivalDate.ToLongDateString();
            departureDate.text = dataItem.DepartureDate.ToLongDateString();
            prepaymentInputField.text = dataItem.Prepayment.ToString();
            exchangeRateInputField.text = dataItem.ExchangeRate.ToString();

            if (clientsDropdown == null &&
                clientsContainer.TryGetById(dataItem.ClientId, out ClientData clientItem) &&
                housesContainer.TryGetById(dataItem.HouseId, out HouseData houseItem))
            {
                clientNameInputField.text = clientItem.Name;
                clientNumberInputField.text = clientItem.ContactNumber;
                clientNotesInputField.text = clientItem.Notes;

                clientRatingToggleGroup.SetMode(ToggleGroupMode.Range);
                clientRatingToggleGroup.SetRange(clientItem.Rating);
                
                houseNameInputField.text = houseItem.Name;
                houseNumberInputField.text = houseItem.Number;
                houseNotesInputField.text = houseItem.Notes;
                houseOwnerNameInputField.text = houseItem.OwnerName;
                houseOwnerNumberInputField.text = houseItem.OwnerContactNumber;
            }
            else
            {
                clientsDropdown.SetSelectedById(dataItem.ClientId);
                housesDropdown.SetSelectedById(dataItem.HouseId);
            }
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
            if (arrivalDate != null) { arrivalDate.text = string.Empty; }
            if (departureDate != null) { departureDate.text = string.Empty; }
            if (exchangeRateInputField != null) { exchangeRateInputField.text = string.Empty; }
        }
    }
}
