using System;
using System.Globalization;
using Extensions.Helpers;
using UnityEngine;

namespace Extensions.ScriptableValues
{
    /// <summary>
    /// Хранилище даты
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DateValue),
        menuName = "Extensions/ScriptableValue/" + nameof(DateValue)
    )]
    public class DateValue : StringValue
    {
        [SerializeField] private bool useNowAsDefault = false;
        [SerializeField] private string dateFormat = "dd.MM.yy";
        
        /// <summary>
        /// Формат даты
        /// </summary>
        public string DateFormat => dateFormat;

        /// <summary>
        /// Использовать текущую дату как дефолтную
        /// </summary>
        public bool UseNowAsDefault => useNowAsDefault;

        protected override void OnEnable()
        {
            if (useNowAsDefault) defaultValue = DateUtils.Format(DateTime.Now);
            base.OnEnable();
        }

        /// <summary>
        /// Установка даты
        /// </summary>
        public override void SetValue(string newValue)
        {
            if (!DateUtils.IsValidDate(newValue))
            {
                Debug.LogError($"Некорректная дата: {newValue}. Ожидаемый формат: {DateFormat}. Значение сброшено до дефолтного");
                ResetToDefault();
                return;
            }

            base.SetValue(newValue);
        }

        /// <summary>
        /// Установка даты
        /// </summary>
        public void SetValue(DateTime dateTime) => SetValue(DateUtils.Format(dateTime, DateFormat));

        /// <summary>
        /// Попытаться получить дату
        /// </summary>
        public bool TryGetDate(out DateTime result) => DateUtils.TryParse(Value, out result);

        /// <summary>
        /// Получить дату
        /// </summary>
        public DateTime GetDate(DateTime defaultDate)
        {
            if (TryGetDate(out DateTime result)) return result;
            else return defaultDate;
        }
        
        /// <summary>
        /// Проверка дефолтного значения
        /// </summary>
        public bool IsDefaultValueValid() => useNowAsDefault || DateUtils.IsValidDate(defaultValue);

        /// <summary>
        /// Проверка даты
        /// </summary>
        public bool IsDateValid(string value)
        {
            return DateTime.TryParseExact(
                value,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime _
            );
        }
    }
}