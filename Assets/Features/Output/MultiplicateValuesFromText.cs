using System;
using System.Globalization;
using Extensions.Generics;
using Extensions.Helpers;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Умножение числовых значений из строк и вывод
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class MultiplicateValuesFromText : AbstractInputField
    {
        [SerializeField] private TMP_InputField firstInput;
        [SerializeField] private TMP_InputField secondInput;

        protected override void OnEnable()
        {
            firstInput.onValueChanged.AddListener(OnValueChanged);
            secondInput.onValueChanged.AddListener(OnValueChanged);

            UpdateResult("");
        }

        protected override void OnDisable()
        {
            firstInput.onValueChanged.RemoveListener(OnValueChanged);
            secondInput.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string _) => UpdateResult(_);

        private void UpdateResult(string _)
        {
            float dollars = Parsers.ParseFloat(firstInput.text);
            float days = Parsers.ParseFloat(secondInput.text, 1);

            if (days < 1) days = 1;

            float total = dollars * days;

            inputField.text = Formatters.FormatFloat(total);
        }
    }
}