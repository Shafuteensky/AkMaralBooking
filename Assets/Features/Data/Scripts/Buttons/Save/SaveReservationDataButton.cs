using System;
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
    public class SaveReservationDataButton : SaveDataButton<ReservationsDataContainer, ReservationSelectionContext>
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
        [SerializeField] private TMP_InputField _notesInputField;
        
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

            float paymentPerDay = Parsers.ParseFloat(_paymentPerDayInputField.text);
            int days = Parsers.ParseInt(_daysInputField.text);
            float prepayment = Parsers.ParseFloat(_prepaymentInputField.text);
            float discount = Parsers.ParseFloat(_discountInputField.text);
            float rate = Parsers.ParseFloat(_exchangeRateInputField.text);
            string notes = EmptyIfDefault(_notesInputField.text);

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
                    rate,
                    notes);
                dataContainer.Add(newHouse);
            }
            else
            {
                ReservationData reservation = selectionContext.GetSelectedData();
                reservation.UpdateData(
                    _housesDropDown.SelectedId,
                    _clientsDropDown.SelectedId,
                    arrivalDateTime,
                    departureDateTime,
                    paymentPerDay,
                    days,
                    prepayment,
                    discount,
                    rate,
                    notes);
                dataContainer.NotifyUpdated(reservation);
            }
        }
    }
}