using System;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Слушатель порядка диапазона дат
    /// </summary>
    public sealed class DateRangeOrderListener : MonoBehaviour
    {
        [SerializeField] private DateValue arrivalDate;
        [SerializeField] private DateValue departureDate;

        private bool isApplying = false;

        private void OnEnable()
        {
            if (arrivalDate != null)
                arrivalDate.onValueChanged += OnDateChanged;

            if (departureDate != null)
                departureDate.onValueChanged += OnDateChanged;

            ValidateDateOrder();
        }

        private void OnDisable()
        {
            if (arrivalDate != null)
                arrivalDate.onValueChanged -= OnDateChanged;

            if (departureDate != null)
                departureDate.onValueChanged -= OnDateChanged;
        }

        private void OnDateChanged(string value)
        {
            ValidateDateOrder();
        }

        /// <summary>
        /// Проверить порядок дат
        /// </summary>
        public void ValidateDateOrder()
        {
            if (isApplying) return;
            if (Logic.IsNull(arrivalDate) || Logic.IsNull(departureDate)) return;

            if (!arrivalDate.TryGetDate(out DateTime arrival)) return;
            if (!departureDate.TryGetDate(out DateTime departure)) return;
            if (departure >= arrival) return;

            isApplying = true;

            string arrivalValue = arrivalDate.Value;
            string departureValue = departureDate.Value;

            arrivalDate.SetValue(departureValue);
            departureDate.SetValue(arrivalValue);

            isApplying = false;
        }
    }
}