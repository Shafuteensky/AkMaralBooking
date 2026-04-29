namespace Extensions.Helpers
{
    /// <summary>
    /// Парсеры данных
    /// </summary>
    public static class Parsers
    {
        /// <summary>
        /// Парс целочисленного значения из строки
        /// </summary>
        /// <param name="text">Строка</param>
        /// <param name="defaultValue">Дефолтное значение на случай ошибки</param>
        /// <returns></returns>
        public static int ParseInt(string text, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(text)) return defaultValue;

            int result = 0;
            bool hasDigits = false;

            foreach (var currentChar in text)
            {
                if (!char.IsDigit(currentChar)) continue;

                hasDigits = true;
                result = result * 10 + (currentChar - '0');
            }

            if (!hasDigits) return defaultValue;

            return result;
        }
    }
}