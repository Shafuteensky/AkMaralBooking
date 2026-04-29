using Extensions.Helpers;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Вычисляет долг: итоговая сумма минус предоплата
    /// и выводит результат в TMP_InputField на текущем объекте
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class DebtCalculator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField totalInput;
        [SerializeField] private TMP_InputField discountInput;
        [SerializeField] private TMP_InputField prepaymentInput;

        private TMP_InputField _debtInput;

        private void Awake() => _debtInput = GetComponent<TMP_InputField>();

        private void OnEnable()
        {
            totalInput.onValueChanged.AddListener(OnValueChanged);
            discountInput.onValueChanged.AddListener(OnValueChanged);
            prepaymentInput.onValueChanged.AddListener(OnValueChanged);

            UpdateDebt();
        }

        private void OnDisable()
        {
            totalInput.onValueChanged.RemoveListener(OnValueChanged);
            discountInput.onValueChanged.RemoveListener(OnValueChanged);
            prepaymentInput.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string _) => UpdateDebt();

        private void UpdateDebt()
        {
            int total = Parsers.ParseInt(totalInput.text, 0);
            int discount = Parsers.ParseInt(discountInput.text, 0);
            int prepayment = Parsers.ParseInt(prepaymentInput.text, 0);
            int debt = total - prepayment - discount;

            if (debt < 0) debt = 0;

            _debtInput.SetTextWithoutNotify(debt.ToString());
        }
    }
}