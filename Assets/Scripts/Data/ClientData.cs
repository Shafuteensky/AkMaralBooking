using System;
using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Данные о клиенте
    /// </summary>
    [Serializable]
    public class ClientData : InMemoryDataItem
    {
        [SerializeField]
        protected string name = string.Empty;

        [SerializeField]
        protected string contactNumber = string.Empty;

        [SerializeField]
        protected int rating = 0;

        [SerializeField]
        protected string notes = string.Empty;

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
        public ClientData(string name, string contactNumber, int rating, string notes)
        {
            this.name = name;
            this.contactNumber = contactNumber;
            this.rating = rating;
            this.notes = notes;
        }
    }
}