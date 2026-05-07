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
        [SerializeField] protected string number = string.Empty;
        [SerializeField] protected string name = string.Empty;
        [SerializeField] protected Color color = Color.white;
        [SerializeField] protected string notes = string.Empty;
        [SerializeField] protected string paymentPerDay = string.Empty;
        [SerializeField] protected string ownerName = string.Empty;
        [SerializeField] protected string ownerContactNumber = string.Empty;

        /// <summary>
        /// Цвет записи
        /// </summary>
        public Color Color => color;
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
        /// Получить сигнатуру дома формата {Название (номер)}
        /// </summary>
        public string Signature => $"{Name} ({Number})";
        
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public HouseData(string name, string number, Color color, string notes, string paymentPerDay, string ownerName, string ownerContactNumber)
        {
            UpdateData(name, number, color, notes, paymentPerDay, ownerName, ownerContactNumber);
        }

        /// <summary>
        /// Обновить данные
        /// </summary>
        public void UpdateData(string name, string number, Color color, string notes, string paymentPerDay, string ownerName, string ownerContactNumber)
        {
            this.name = name;
            this.number = number;
            this.color = color;
            this.notes = notes;
            this.paymentPerDay = paymentPerDay;
            this.ownerName = ownerName;
            this.ownerContactNumber = ownerContactNumber;
        }
    }
}