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

            UpdateResult();
        }

        protected override void OnDisable()
        {
            firstInput.onValueChanged.RemoveListener(OnValueChanged);
            secondInput.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string _) => UpdateResult();

        private void UpdateResult()
        {
            int dollars = Parsers.ParseInt(firstInput.text, 0);
            int days = Parsers.ParseInt(secondInput.text, 1);

            if (days < 1) days = 1;

            int total = dollars * days;

            inputField.SetTextWithoutNotify(total.ToString());
        }
    }
}