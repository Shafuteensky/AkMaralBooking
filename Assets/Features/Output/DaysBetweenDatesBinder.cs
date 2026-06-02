using System;
using Extensions.ScriptableValues;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Вычисляет количество дней между двумя датами из TMP_InputField и записывает результат в текущий TMP_InputField
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public sealed class DaysBetweenDatesBinder : MonoBehaviour
    {
        [SerializeField] private DateValue arrivalDateValue;
        [SerializeField] private DateValue departureDateValue;

        [Header("Режим подсчёта"), Space]
        [Tooltip("Если активно — считаются ночи (минимум 1), иначе включительно прибытие и отбытие")]
        [SerializeField] private BoolValue countNightsMode;

        private TMP_InputField targetInput;

        private void Awake()
        {
            targetInput = GetComponent<TMP_InputField>();
        }

        private void OnEnable()
        {
            if (arrivalDateValue != null)
                arrivalDateValue.onValueChanged += OnDateChanged;

            if (departureDateValue != null)
                departureDateValue.onValueChanged += OnDateChanged;

            if (countNightsMode != null)
                countNightsMode.onValueChanged += OnModeChanged;

            OnDateChanged();
        }

        private void OnDisable()
        {
            if (arrivalDateValue != null)
                arrivalDateValue.onValueChanged -= OnDateChanged;

            if (departureDateValue != null)
                departureDateValue.onValueChanged -= OnDateChanged;

            if (countNightsMode != null)
                countNightsMode.onValueChanged -= OnModeChanged;
        }

        private void OnModeChanged(bool _) => OnDateChanged();

        /// <summary>
        /// Пересчитывает дни при изменении любой даты
        /// </summary>
        private void OnDateChanged(string _ = null)
        {
            if (arrivalDateValue == null || departureDateValue == null) return;

            int days = Math.Abs((departureDateValue.GetDate() - arrivalDateValue.GetDate()).Days);

            // Режим ночей: количество ночей между датами, но не меньше одних суток.
            // Иначе — включительный подсчёт суток (прибытие и отбытие считаются отдельными днями).
            int result = countNightsMode != null && countNightsMode.Value
                ? Math.Max(days, 1)
                : days + 1;

            targetInput.SetTextWithoutNotify(result.ToString());
        }
    }
}