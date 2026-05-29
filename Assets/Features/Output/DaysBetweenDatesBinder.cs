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

            OnDateChanged();
        }

        private void OnDisable()
        {
            if (arrivalDateValue != null)
                arrivalDateValue.onValueChanged -= OnDateChanged;

            if (departureDateValue != null)
                departureDateValue.onValueChanged -= OnDateChanged;
        }

        /// <summary>
        /// Пересчитывает дни при изменении любой даты
        /// </summary>
        private void OnDateChanged(string _ = null)
        {
            if (arrivalDateValue == null || departureDateValue == null) return;

            int days = (departureDateValue.GetDate() - arrivalDateValue.GetDate()).Days;
            targetInput.SetTextWithoutNotify((Math.Abs(days) + 1).ToString());
        }
    }
}