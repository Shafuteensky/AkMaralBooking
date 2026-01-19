using System;
using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Данные о записи аренды
    /// </summary>
    [Serializable]
    public class ReservationData : InMemoryDataItem
    {
        [SerializeField]
        protected string clientId = default;
        [SerializeField]
        protected Color color = Color.white;
        
        [SerializeField]
        protected string arrivalDateIso = string.Empty;
        [SerializeField]
        protected string departureDateIso = string.Empty;

        [SerializeField]
        protected float paymentPerDay = 0f;
        [SerializeField]
        protected int days = 0;
        [SerializeField]
        protected float prepayment = 0f;
        [SerializeField]
        protected float exchangeRate = 0f;

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId => clientId;
        /// <summary>
        /// Цвет записи
        /// </summary>
        public Color Color => color;

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
        /// Курс валют (доллар/сом)
        /// </summary>
        public float ExchangeRate => exchangeRate;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ReservationData(
            string clientId,
            Color color,
            DateTime arrivalDate,
            DateTime departureDate,
            float paymentPerDay,
            int days,
            float prepayment,
            float exchangeRate)
        {
            this.clientId = clientId;
            this.color = color;
            this.arrivalDateIso = arrivalDate.ToString("o");
            this.departureDateIso = departureDate.ToString("o");
            this.paymentPerDay = paymentPerDay;
            this.days = days;
            this.prepayment = prepayment;
            this.exchangeRate = exchangeRate;
        }
    }
}
