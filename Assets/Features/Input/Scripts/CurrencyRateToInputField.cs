using System.Collections;
using Extensions.Generics;
using StarletBooking.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Загружает курс USD → KGS и выводит его в TMP_InputField
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CurrencyRateToInputField : AbstractInputField
    {
        private const string Url = "https://api.exchangerate.host/latest?base=USD&symbols=KGS";

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!string.IsNullOrEmpty(inputField.text)) return;

            StartCoroutine(LoadRate());
        }

        public override void OnInputFieldValueUpdated(string value)
        {
        }

        private IEnumerator LoadRate()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(Url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Currency load error: " + request.error);
                    SetDefaultRate();
                    yield break;
                }

                Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

                if (response == null || response.rates == null || response.rates.KGS <= 0)
                {
                    SetDefaultRate();
                    yield break;
                }

                SetRate(response.rates.KGS);
            }
        }

        private void SetRate(float value)
        {
            inputField.SetTextWithoutNotify(value.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("ru-RU")));
        }

        private void SetDefaultRate()
        {
            SetRate(DataBus.instance.DefaultExchangeRate.Value);
        }

        [System.Serializable]
        private sealed class Response
        {
            public Rates rates;
        }

        [System.Serializable]
        private sealed class Rates
        {
            public float KGS;
        }
    }
}