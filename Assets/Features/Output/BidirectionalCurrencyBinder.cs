using Extensions.Helpers;
using Extensions.Log;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Обратное направление конвертации валют: ввод в поле валюты пересчитывает поле долларов.
    /// Прямое направление (доллары → валюта) обеспечивает <see cref="MultiplicateValueFromText"/> на secondaryField.
    /// </summary>
    public sealed class BidirectionalCurrencyBinder : MonoBehaviour
    {
        [SerializeField] private TMP_InputField primaryField;
        [SerializeField] private TMP_InputField secondaryField;
        [SerializeField] private TMP_InputField rateField;

        private void OnEnable() => secondaryField?.onValueChanged.AddListener(OnSecondaryChanged);

        private void OnDisable() => secondaryField?.onValueChanged.RemoveListener(OnSecondaryChanged);

        private void OnSecondaryChanged(string _)
        {
            if (Logic.IsNull(primaryField) || Logic.IsNull(secondaryField) || Logic.IsNull(rateField)) return;

            float rate = Parsers.ParseFloat(rateField.text, 1);

            if (Mathf.Approximately(rate, 0f))
            {
                ServiceDebug.LogWarning("Курс валюты равен нулю — обратный пересчёт невозможен");
                return;
            }

            float secondaryValue = Parsers.ParseFloat(secondaryField.text, 0);
            primaryField.SetTextWithoutNotify(Formatters.FormatFloat(secondaryValue / rate));
        }
    }
}
