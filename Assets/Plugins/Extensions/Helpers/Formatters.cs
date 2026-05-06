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
        public static string FormatFloat(float value) => value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}