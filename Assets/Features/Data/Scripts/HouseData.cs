using System;
using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Данные о доме
    /// </summary>
    [Serializable]
    public class HouseData : InMemoryDataEntry
    {
        [SerializeField] protected string name = string.Empty;
        [SerializeField] protected string number = string.Empty;
        [SerializeField] protected string notes = string.Empty;
        [SerializeField] protected string paymentPerDay = string.Empty;
        [SerializeField] protected string ownerName = string.Empty;
        [SerializeField] protected string ownerContactNumber = string.Empty;

        /// <summary>
        /// Наименование дома
        /// </summary>
        public string Name => name;
        /// <summary>
        /// Номер дома
        /// </summary>
        public string Number => number;
        /// <summary>
        /// Заметки о доме
        /// </summary>
        public string Notes => notes;
        /// <summary>
        /// Стоимость аренды за день
        /// </summary>
        public string PaymentPerDay => paymentPerDay;
        /// <summary>
        /// Имя владельца
        /// </summary>
        public string OwnerName => ownerName;
        /// <summary>
        /// Контактный номер владельца
        /// </summary>
        public string OwnerContactNumber => ownerContactNumber;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HouseData(string name, string number, string notes, string paymentPerDay, string ownerName, string ownerContactNumber)
        {
            UpdateData(name, number, notes, paymentPerDay, ownerName, ownerContactNumber);
        }

        /// <summary>
        /// Обновить данные
        /// </summary>
        public void UpdateData(string name, string number, string notes, string paymentPerDay, string ownerName, string ownerContactNumber)
        {
            this.name = name;
            this.number = number;
            this.notes = notes;
            this.paymentPerDay = paymentPerDay;
            this.ownerName = ownerName;
            this.ownerContactNumber = ownerContactNumber;
        }
    }
}