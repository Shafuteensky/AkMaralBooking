using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Проверки данных
    /// </summary>
    public static class DataHelpers
    {
        private const string EMPTY_STRING = "<color=yellow><i>ДАННЫЕ ОТСУТСТВУЮТ</i></color>";
        private const string NOT_FOUND_STRING = "<color=red><i>ОШИБКА ДАННЫХ</i></color>";
        
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
        /// Утеряные данные (цвет)
        /// </summary>
        public static Color NotFoundColor => NOT_FOUND_COLOR;
        
        /// <summary>
        /// Строка данных InMemoryDataItem
        /// </summary>
        /// <returns>Информативная строка</returns>
        public static string GetString(string dataString)
        {
            return string.IsNullOrEmpty(dataString) ? EMPTY_STRING : dataString;
        }
    }
}