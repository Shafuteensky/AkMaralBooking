using System;
using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Данные о записи аренды
    /// </summary>
    [Serializable]
    public class ReservationData : InMemoryDataEntry
    {
        [SerializeField] protected string houseId = default;
        [SerializeField] protected string clientId = default;
        
        [SerializeField] protected string arrivalDateIso = string.Empty;
        [SerializeField] protected string departureDateIso = string.Empty;

        [SerializeField] protected float paymentPerDay = 0f;
        [SerializeField] protected int days = 0;
        [SerializeField] protected float prepayment = 0f;
        [SerializeField] protected float discount = 0f;
        [SerializeField] protected float exchangeRate = 0f;
        [SerializeField] protected string notes = string.Empty;

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public string HouseId => houseId;
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId => clientId;

        /// <summary>
        /// Дата прибытия (ISO)
        /// </summary>
        public DateTime ArrivalDate => DateTime.Parse(arrivalDateIso);
        /// <summary>
        /// Дата отбытия (ISO)
        /// </summary>
        public DateTime DepartureDate => DateTime.Parse(departureDateIso);

        /// <summary>
        /// Оплата за день
        /// </summary>
        public float PaymentPerDay => paymentPerDay;
        /// <summary>
        /// Дней аренды
        /// </summary>
        public int Days => days;
        /// <summary>
        /// Величина предоплаты
        /// </summary>
        public float Prepayment => prepayment;
        /// <summary>
        /// Персональная скидка
        /// </summary>
        public float Discount => discount;
        /// <summary>
        /// Курс валют (доллар/сом)
        /// </summary>
        public float ExchangeRate => exchangeRate;
        
        /// <summary>
        /// Заметки
        /// </summary>
        public string Notes => notes;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ReservationData(
            string houseId,
            string clientId,
            DateTime arrivalDate,
            DateTime departureDate,
            float paymentPerDay,
            int days,
            float prepayment,
            float discount,
            float exchangeRate,
            string notes)
        {
            UpdateData(houseId, clientId, arrivalDate, departureDate, paymentPerDay, days, prepayment, discount, exchangeRate, notes);
        }
        
        /// <summary>
        /// Обновить данные
        /// </summary>
        public void UpdateData(
            string houseId,
            string clientId,
            DateTime arrivalDate,
            DateTime departureDate,
            float paymentPerDay,
            int days,
            float prepayment,
            float discount,
            float exchangeRate,
            string notes)
        {
            this.houseId = houseId;
            this.clientId = clientId;
            this.arrivalDateIso = arrivalDate.ToString("o");
            this.departureDateIso = departureDate.ToString("o");
            this.paymentPerDay = paymentPerDay;
            this.days = days;
            this.prepayment = prepayment;
            this.discount = discount;
            this.exchangeRate = exchangeRate;
            this.notes = notes;
        }
    }
}
