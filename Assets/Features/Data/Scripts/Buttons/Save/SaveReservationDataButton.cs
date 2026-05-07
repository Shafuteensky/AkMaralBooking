using System;
using System.Globalization;
using Extensions.Helpers;
using Extensions.Log;
using StarletBooking.UI;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Сохранение записи об аренде
    /// </summary>
    public class SaveReservationDataButton : SaveDataButton<ReservationsDataContainer, ReservationSingleSelectionContext>
    {
        [Header("Поля данных"), Space]
        [SerializeField] private DropdownIdBinder _housesDropDown;
        [SerializeField] private DropdownIdBinder _clientsDropDown;
        [SerializeField] private TMP_InputField arrivalDate;
        [SerializeField] private TMP_InputField departureDate;
        [SerializeField] private TMP_InputField _paymentPerDayInputField;
        [SerializeField] private TMP_InputField _daysInputField;
        [SerializeField] private TMP_InputField _prepaymentInputField;
        [SerializeField] private TMP_InputField _discountInputField;
        [SerializeField] private TMP_InputField _exchangeRateInputField;
        
        public override void OnButtonClickAction()
        {
            if (dataContainer == null
                || selectionContext == null)
            {
                ServiceDebug.LogError("Отсутствует ссылка на контейнер данных, запись не добавлена");
                return;
            }
            
            if (_clientsDropDown == null 
                || _paymentPerDayInputField == null
                || _daysInputField == null 
                || _prepaymentInputField == null
                || _discountInputField == null
                || _exchangeRateInputField == null )
            {
                ServiceDebug.LogError("Не все ссылки на поля ввода заполнены, запись не добавлена");
                return;
            }

            float.TryParse(_paymentPerDayInputField.text, out var paymentPerDay);
            int.TryParse(_daysInputField.text, out var days);
            float.TryParse(_prepaymentInputField.text, out var prepayment);
            float.TryParse(_discountInputField.text, out var discount);
            float.TryParse(_exchangeRateInputField.text, out var rate);

            if (!DateUtils.TryParse(arrivalDate.text, out DateTime arrivalDateTime)) arrivalDateTime = DateTime.Now;
            if (!DateUtils.TryParse(departureDate.text, out DateTime departureDateTime)) departureDateTime = DateTime.Now;
            
            if (!selectionContext.HasSelection)
            {
                ReservationData newHouse = new ReservationData(
                    _housesDropDown.SelectedId,
                    _clientsDropDown.SelectedId,
                    arrivalDateTime,
                    departureDateTime,
                    paymentPerDay,
                    days,
                    prepayment,
                    discount,
                    rate);
                dataContainer.Add(newHouse);
            }
            else
            {
                selectionContext.GetSelectedData().UpdateData(
                    _housesDropDown.SelectedId,
                    _clientsDropDown.SelectedId,
                    arrivalDateTime,
                    departureDateTime,
                    paymentPerDay,
                    days,
                    prepayment,
                    discount,
                    rate);
                dataContainer.NotifyUpdated();
            }
        }
    }
}