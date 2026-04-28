using System;
using Extensions.Helpers;
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
        [SerializeField] private TMP_InputField startDateInput;
        [SerializeField] private TMP_InputField endDateInput;

        private TMP_InputField targetInput;

        private void Awake()
        {
            targetInput = GetComponent<TMP_InputField>();
        }

        private void OnEnable()
        {
            if (startDateInput != null)
                startDateInput.onValueChanged.AddListener(OnDateChanged);

            if (endDateInput != null)
                endDateInput.onValueChanged.AddListener(OnDateChanged);
        }

        private void OnDisable()
        {
            if (startDateInput != null)
                startDateInput.onValueChanged.RemoveListener(OnDateChanged);

            if (endDateInput != null)
                endDateInput.onValueChanged.RemoveListener(OnDateChanged);
        }

        /// <summary>
        /// Пересчитывает дни при изменении любой даты
        /// </summary>
        private void OnDateChanged(string _)
        {
            if (startDateInput == null || endDateInput == null) return;

            if (string.IsNullOrEmpty(startDateInput.text) ||
                string.IsNullOrEmpty(endDateInput.text))
            {
                targetInput.text = string.Empty;
                return;
            }

            if (!DateUtils.TryParse(startDateInput.text, out DateTime startDate) |
                !DateUtils.TryParse(endDateInput.text, out DateTime endDate))
            {
                targetInput.text = string.Empty;
                return;
            }
            
            int days = (endDate - startDate).Days;
            targetInput.text = days.ToString();
        }
    }
}