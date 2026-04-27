using System;
using System.Globalization;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Проверки данных
    /// </summary>
    public static class DataHelpers
    {
        private const string EMPTY_STRING = "ДАННЫЕ ОТСУТСТВУЮТ";
        private const string NOT_FOUND_STRING = "ОШИБКА ДАННЫХ";
        
        private const string DATE_FORMAT = "dd.MM.yy";

        private static readonly Color NOT_FOUND_COLOR = Color.whiteSmoke;
        
        /// <summary>
        /// Пустые данные (строки)
        /// </summary>
        public static string EmptyString => EMPTY_STRING;
        /// <summary>
        /// Утеряные данные (строки)
        /// </summary>
        public static string NotFoundString => NOT_FOUND_STRING;
        
        /// <summary>
        /// Формат данных даты
        /// </summary>
        public static string DateFormat => DATE_FORMAT;
        
        /// <summary>
        /// Утеряные данные (цвет)
        /// </summary>
        public static Color NotFoundColor => NOT_FOUND_COLOR;
        
        /// <summary>
        /// Строка данных InMemoryDataItem
        /// </summary>
        /// <returns>Информативная строка</returns>
        public static string GetString(string dataString)
        {
            if (string.IsNullOrEmpty(dataString)) return EMPTY_STRING;
            return dataString;
        }
        
        /// <summary>
        /// Пытается распарсить дату из строки в формате DateFormat
        /// </summary>
        public static bool TryGetDate(string value, out DateTime date)
        {
            return DateTime.TryParseExact(
                value,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date);
        }
    }
}