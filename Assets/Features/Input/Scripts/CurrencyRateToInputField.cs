using System.Collections;
using Extensions.Coroutines;
using Extensions.Generics;
using Extensions.Helpers;
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

            CoroutineDelay.Run(this, CheckAndLoadRate);
        }

        private void CheckAndLoadRate()
        {
            if (!string.IsNullOrEmpty(inputField.text)) return;

            StartCoroutine(LoadRate());
        }

        protected override void OnInputFieldValueUpdated(string value) { }

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
            // Курс — точное долларовое значение: 3 знака после запятой
            inputField.SetTextWithoutNotify(Formatters.FormatFloat(value, 3));
        }

        private void SetDefaultRate()
        {
            SetRate(DataBus.Instance.DefaultExchangeRate.Value);
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