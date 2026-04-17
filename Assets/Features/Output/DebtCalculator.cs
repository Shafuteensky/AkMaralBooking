using TMPro;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// Вычисляет долг: итоговая сумма минус предоплата
    /// и выводит результат в TMP_InputField на текущем объекте
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class DebtCalculator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField totalInput;
        [SerializeField] private TMP_InputField prepaymentInput;

        private TMP_InputField _debtInput;

        private void Awake() => _debtInput = GetComponent<TMP_InputField>();

        private void OnEnable()
        {
            totalInput.onValueChanged.AddListener(OnValueChanged);
            prepaymentInput.onValueChanged.AddListener(OnValueChanged);

            UpdateDebt();
        }

        private void OnDisable()
        {
            totalInput.onValueChanged.RemoveListener(OnValueChanged);
            prepaymentInput.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string _) => UpdateDebt();

        private void UpdateDebt()
        {
            int total = ParseInt(totalInput.text, 0);
            int prepayment = ParseInt(prepaymentInput.text, 0);
            int debt = total - prepayment;

            if (debt < 0) debt = 0;

            _debtInput.SetTextWithoutNotify(debt + "$");
        }

        private int ParseInt(string text, int defaultValue)
        {
            if (string.IsNullOrEmpty(text)) return defaultValue;

            int result = 0;
            bool hasDigits = false;

            foreach (var currentChar in text)
            {
                if (!char.IsDigit(currentChar)) continue;

                hasDigits = true;
                result = result * 10 + (currentChar - '0');
            }

            if (!hasDigits) return defaultValue;

            return result;
        }
    }
}