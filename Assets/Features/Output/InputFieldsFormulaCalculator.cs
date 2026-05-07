using System.Collections.Generic;
using Extensions.Generics;
using Extensions.Helpers;
using TMPro;
using UnityEngine;

namespace Extensions.UI
{
    /// <summary>
    /// Подсчет значения из полей <see cref="TMP_InputField"/>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class InputFieldsFormulaCalculator : AbstractInputField
    {
        [SerializeField] private TMP_InputField paymentPerDay;
        [SerializeField] private TMP_InputField numberOfDays;
        [SerializeField] private List<TMP_InputField> subtractValues;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Subscribe(paymentPerDay);
            Subscribe(numberOfDays);
            foreach (TMP_InputField field in subtractValues)
                Subscribe(field);

            UpdateResult();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Unsubscribe(paymentPerDay);
            Unsubscribe(numberOfDays);
            foreach (TMP_InputField field in subtractValues)
                Unsubscribe(field);
        }

        private void Subscribe(TMP_InputField field)
        {
            if (field == null) return;
            field.onValueChanged.AddListener(OnValueChanged);
        }

        private void Unsubscribe(TMP_InputField field)
        {
            if (field == null) return;
            field.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string value) => UpdateResult();

        private void UpdateResult()
        {
            float result = GetValue(paymentPerDay, 0f) * GetValue(numberOfDays, 1f) - GetSum(subtractValues);

            if (inputField == null) return;
            inputField.text = Formatters.FormatFloat(result);
        }

        private float GetSum(List<TMP_InputField> fields)
        {
            float result = 0f;
            foreach (TMP_InputField field in fields)
                result += GetValue(field, 0f);

            return result;
        }

        private float GetValue(TMP_InputField field, float defaultValue)
        {
            if (field == null) return defaultValue;
            if (!float.TryParse(field.text, out float value)) return defaultValue;

            return value;
        }
    }
}