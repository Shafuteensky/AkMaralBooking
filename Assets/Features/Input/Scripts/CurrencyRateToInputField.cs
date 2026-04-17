using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Загружает курс USD → KGS и выводит его в TMP_InputField
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class CurrencyRateToInputField : MonoBehaviour
    {
        private const string Url = "https://api.exchangerate.host/latest?base=USD&symbols=KGS";

        protected TMP_InputField _inputField;

        protected virtual void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        protected virtual void OnEnable()
        {
            if (!string.IsNullOrEmpty(_inputField.text))
            {
                return;
            }

            StartCoroutine(LoadRate());
        }

        protected IEnumerator LoadRate()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(Url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Currency load error: " + request.error);
                    SetText("—");
                    yield break;
                }

                Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

                if (response == null || response.rates == null)
                {
                    SetText("—");
                    yield break;
                }

                float rate = response.rates.KGS;
                SetText("1$ = " + rate.ToString("0.##") + " сом");
            }
        }

        /// <summary>
        /// Устанавливает текст без вызова событий TMP_InputField
        /// </summary>
        protected void SetText(string value)
        {
            _inputField.SetTextWithoutNotify(value);
        }

        [System.Serializable]
        protected class Response
        {
            public Rates rates;
        }

        [System.Serializable]
        protected class Rates
        {
            public float KGS;
        }
    }
}