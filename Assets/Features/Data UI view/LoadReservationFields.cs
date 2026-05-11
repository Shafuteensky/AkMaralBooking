using Extensions.Helpers;
using StarletBooking.Data.View;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Загружает базовые поля бронирования: даты, дни, платежи и курс
    /// </summary>
    public sealed class LoadReservationFields : SelectionContextViewLoader<ReservationData>
    {
        private const string EDIT_INTEGER_FORMAT = "0";
        private const string VIEW_INTEGER_FORMAT = "N0";

        [Header("Формат вывода численных данных"), Space]
        [SerializeField] private bool viewOutputFormat = false;

        [Header("Заполняемые поля"), Space]
        [SerializeField] private TMP_InputField paymentPerDayInputField;
        [SerializeField] private TMP_InputField daysInputField;
        [SerializeField] private TMP_InputField prepaymentInputField;
        [SerializeField] private TMP_InputField discountInputField;
        [SerializeField] private TMP_InputField exchangeRateInputField;
        [SerializeField] private TMP_InputField arrivalDate;
        [SerializeField] private TMP_InputField departureDate;

        private string GetFormat => viewOutputFormat ? VIEW_INTEGER_FORMAT : EDIT_INTEGER_FORMAT;
        
        protected override void ApplyToView(ReservationData dataItem)
        {
            if (Logic.IsNull(paymentPerDayInputField) ||
                Logic.IsNull(daysInputField) ||
                Logic.IsNull(arrivalDate) ||
                Logic.IsNull(departureDate) ||
                Logic.IsNull(exchangeRateInputField) ||
                Logic.IsNull(prepaymentInputField) ||
                Logic.IsNull(discountInputField))
            { return; }

            arrivalDate.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(DateUtils.Format(dataItem.ArrivalDate));
            departureDate.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(DateUtils.Format(dataItem.DepartureDate));

            daysInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Days.ToString());

            paymentPerDayInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.PaymentPerDay.ToString(GetFormat));
            discountInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Discount.ToString(GetFormat));
            prepaymentInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Prepayment.ToString(GetFormat));
            
            exchangeRateInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.ExchangeRate.ToString(GetFormat));
        }

        protected override void ApplyEmpty()
        {
            if (paymentPerDayInputField != null) { paymentPerDayInputField.text = string.Empty; }
            if (daysInputField != null) { daysInputField.text = string.Empty; }
            
            if (prepaymentInputField != null) { prepaymentInputField.text = string.Empty; }
            
            if (discountInputField != null) { discountInputField.text = string.Empty; }
            if (arrivalDate != null) { arrivalDate.text = string.Empty; }
            if (departureDate != null) { departureDate.text = string.Empty; }
            
            if (exchangeRateInputField != null) { exchangeRateInputField.text = string.Empty; }
        }
    }
}