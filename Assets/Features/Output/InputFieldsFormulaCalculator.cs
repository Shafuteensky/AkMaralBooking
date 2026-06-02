using System.Collections.Generic;
using Extensions.Generics;
using Extensions.Helpers;
using Extensions.ScriptableValues;
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
        [SerializeField] private List<TMP_InputField> additionalUpdateSources;
        [SerializeField] private List<StringValue> valueUpdateSources;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Subscribe(paymentPerDay);
            Subscribe(numberOfDays);
            Subscribe(subtractValues);
            Subscribe(additionalUpdateSources);
            foreach (StringValue scriptableValue in valueUpdateSources)
                scriptableValue.onValueChanged += OnValueChanged;

            UpdateResult();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Unsubscribe(paymentPerDay);
            Unsubscribe(numberOfDays);
            Unsubscribe(subtractValues);
            Unsubscribe(additionalUpdateSources);
            foreach (StringValue scriptableValue in valueUpdateSources)
                scriptableValue.onValueChanged -= OnValueChanged;
        }

        private void Subscribe(TMP_InputField field)
        {
            if (field == null) return;
            field.onValueChanged.AddListener(OnValueChanged);
        }

        private void Subscribe(List<TMP_InputField> fields)
        {
            if (fields == null) return;
            foreach (TMP_InputField field in fields)
                Subscribe(field);
        }

        private void Unsubscribe(TMP_InputField field)
        {
            if (field == null) return;
            field.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void Unsubscribe(List<TMP_InputField> fields)
        {
            if (fields == null) return;
            foreach (TMP_InputField field in fields)
                Unsubscribe(field);
        }

        private void OnValueChanged(string value) => UpdateResult();

        private void UpdateResult()
        {
            float result = GetValue(paymentPerDay, 0f) * GetValue(numberOfDays, 1f) - GetSum(subtractValues);

            if (inputField == null) return;
            // Итог по оплате считается в долларах: 3 знака после запятой
            inputField.text = Formatters.FormatFloat(result, 3);
        }

        private float GetSum(List<TMP_InputField> fields)
        {
            if (fields == null) return 0f;

            float result = 0f;
            foreach (TMP_InputField field in fields)
                result += GetValue(field, 0f);

            return result;
        }

        private float GetValue(TMP_InputField field, float defaultValue)
        {
            if (field == null) return defaultValue;
            float value = Parsers.ParseFloat(field.text, defaultValue);

            return value;
        }
    }
}