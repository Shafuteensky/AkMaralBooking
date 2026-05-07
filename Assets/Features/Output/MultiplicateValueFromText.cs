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
    public class MultiplicateValueFromText : AbstractInputField
    {
        [SerializeField] private TMP_InputField firstInput;
        [SerializeField] private TMP_InputField secondInput;

        protected override void OnEnable()
        {
            firstInput?.onValueChanged.AddListener(OnValueChanged);
            secondInput?.onValueChanged.AddListener(OnValueChanged);

            UpdateResult();
        }

        protected override void OnDisable()
        {
            firstInput?.onValueChanged.RemoveListener(OnValueChanged);
            secondInput?.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string _) => UpdateResult();

        private void UpdateResult(string _ = "")
        {
            if (Logic.IsNull(firstInput) || Logic.IsNull(secondInput)) return;
            
            float firstValue = Parsers.ParseFloat(firstInput.text, 1);
            float secondValue = Parsers.ParseFloat(secondInput.text, 1);

            float total = firstValue * secondValue;

            inputField.text = Formatters.FormatFloat(total);
        }
    }
}