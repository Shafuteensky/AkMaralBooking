using TMPro;
using UnityEngine;

namespace Project.UI.Output
{
    /// <summary>
    /// Вычисляет итоговую стоимость: сумма в долларах * количество дней
    /// и выводит результат в TMP_InputField на текущем объекте
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class TotalPriceCalculator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField dollarInput;
        [SerializeField] private TMP_InputField daysInput;

        private TMP_InputField _resultInput;

        private void Awake() => _resultInput = GetComponent<TMP_InputField>();

        private void OnEnable()
        {
            dollarInput.onValueChanged.AddListener(OnValueChanged);
            daysInput.onValueChanged.AddListener(OnValueChanged);

            UpdateResult();
        }

        private void OnDisable()
        {
            dollarInput.onValueChanged.RemoveListener(OnValueChanged);
            daysInput.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string _) => UpdateResult();

        private void UpdateResult()
        {
            int dollars = ParseInt(dollarInput.text, 0);
            int days = ParseInt(daysInput.text, 1);

            if (days < 1) days = 1;

            int total = dollars * days;

            _resultInput.SetTextWithoutNotify(total + "$");
        }

        private int ParseInt(string text, int defaultValue)
        {
            if (string.IsNullOrEmpty(text)) return defaultValue;

            int result = 0;
            bool hasDigits = false;

            foreach (var c in text)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                hasDigits = true;
                result = result * 10 + (c - '0');
            }

            if (!hasDigits) return defaultValue;

            return result;
        }
    }
}