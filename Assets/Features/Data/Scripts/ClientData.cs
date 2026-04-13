using System;
using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Данные о клиенте
    /// </summary>
    [Serializable]
    public class ClientData : InMemoryDataEntry
    {
        [SerializeField]
        protected string name = string.Empty;
        [SerializeField]
        protected Color color = Color.white;
        [SerializeField]
        protected string contactNumber = string.Empty;
        [SerializeField]
        protected int rating = 0;
        [SerializeField]
        protected string notes = string.Empty;
        
        /// <summary>
        /// Цвет записи
        /// </summary>
        public Color Color => color;
        /// <summary>
        /// ФИО
        /// </summary>
        public string Name => name;
        /// <summary>
        /// Контактный номер
        /// </summary>
        public string ContactNumber => contactNumber;
        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Rating => rating;
        /// <summary>
        /// Заметки
        /// </summary>
        public string Notes => notes;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">ФИО</param>
        /// <param name="contactNumber">Контактный номер</param>
        /// <param name="rating">Рейтинг</param>
        /// <param name="notes">Заметки</param>
        public ClientData(string name, Color color, string contactNumber, int rating, string notes)
        {
            UpdateData(name, color, contactNumber, rating, notes);
        }

        /// <summary>
        /// Обновить данные
        /// </summary>
        public void UpdateData(string name, Color color, string contactNumber, int rating, string notes)
        {
            this.name = name;
            this.color = color;
            this.contactNumber = contactNumber;
            this.rating = rating;
            this.notes = notes;
        }
    }
}