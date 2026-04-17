using System;
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
        [SerializeField] private TMP_InputField _exchangeRateInputField;
        
        public override void OnButtonClick()
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
                || _exchangeRateInputField == null )
            {
                ServiceDebug.LogError("Не все ссылки на поля ввода заполнены, запись не добавлена");
                return;
            }

            float paymentPerDay = 0f;
            float.TryParse(_paymentPerDayInputField.text, out paymentPerDay);
            int days = 1;
            int.TryParse(_daysInputField.text, out days);
            float prepayment = 0f;
            float.TryParse(_prepaymentInputField.text, out prepayment);
            float rate = 0f;
            float.TryParse(_exchangeRateInputField.text, out rate);
            
            if (!selectionContext.HasSelection)
            {
                ReservationData newHouse = new ReservationData(
                    _housesDropDown.SelectedId,
                    _clientsDropDown.SelectedId,
                    DateTime.Today,
                    DateTime.Today,
                    paymentPerDay,
                    days,
                    prepayment,
                    rate);
                dataContainer.Add(newHouse);
            }
            else
            {
                selectionContext.GetSelectedData().UpdateData(
                    _housesDropDown.SelectedId,
                    _clientsDropDown.SelectedId,
                    DateTime.Today,
                    DateTime.Today,
                    paymentPerDay,
                    days,
                    prepayment,
                    rate);
                dataContainer.NotifyUpdated();
            }
        }
    }
}