using Extensions.Helpers;
using StarletBooking.Data.View;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Загрузка данных об облате для записи аренды
    /// </summary>
    public sealed class LoadReservationPaymentData : SelectionContextViewLoader<ReservationData>
    {
        private const string EDIT_INTEGER_FORMAT = "0";
        private const string VIEW_INTEGER_FORMAT = "N0";

        [Header("Формат вывода численных данных"), Space]
        [SerializeField] private bool viewOutputFormat = false;

        [Header("Заполняемые поля"), Space]
        [SerializeField] private TMP_InputField paymentPerDayInputField;
        [SerializeField] private TMP_InputField prepaymentInputField;
        [SerializeField] private TMP_InputField discountInputField;
        [SerializeField] private TMP_InputField exchangeRateInputField;

        private string GetFormat => viewOutputFormat ? VIEW_INTEGER_FORMAT : EDIT_INTEGER_FORMAT;
        
        protected override void ApplyToView(ReservationData dataItem)
        {
            if (Logic.IsNull(paymentPerDayInputField) ||
                Logic.IsNull(exchangeRateInputField) ||
                Logic.IsNull(prepaymentInputField) ||
                Logic.IsNull(discountInputField))
            { return; }

            exchangeRateInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.ExchangeRate.ToString(GetFormat));
            
            paymentPerDayInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.PaymentPerDay.ToString(GetFormat));
            discountInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Discount.ToString(GetFormat));
            prepaymentInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Prepayment.ToString(GetFormat));
        }

        protected override void ApplyEmpty()
        {
            if (exchangeRateInputField != null) { exchangeRateInputField.text = string.Empty; }
            
            if (paymentPerDayInputField != null) { paymentPerDayInputField.text = string.Empty; }
            if (prepaymentInputField != null) { prepaymentInputField.text = string.Empty; }
            if (discountInputField != null) { discountInputField.text = string.Empty; }
        }
    }
}