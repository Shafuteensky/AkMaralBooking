using System.Globalization;

namespace Extensions.Helpers
{
    /// <summary>
    /// Форматирование данных
    /// </summary>
    public static class Formatters
    {
        /// <summary>
        /// Форматирование float-значения к 12.34
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="decimals">Максимальное число знаков после запятой</param>
        public static string FormatFloat(float value, int decimals = 2)
        {
            string format = decimals > 0 ? "0." + new string('#', decimals) : "0";
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Форматирование string-значения к Hello
        /// </summary>
        public static string FormatString(string value) => char.ToUpper(value[0]) + value[1..];
    }
}