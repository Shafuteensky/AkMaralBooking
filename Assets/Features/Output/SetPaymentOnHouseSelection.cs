using Extensions.Log;
using StarletBooking.Data;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Вывод стоимости аренды дома при изменении контекста выбора
    /// </summary>
    public class SetPaymentOnHouseSelection : MonoBehaviour
    {
        [SerializeField] protected HouseSelectionContext houseSelectionContext;
        [SerializeField] private TMP_InputField paymentInputField;

        private void OnEnable()
        {
            houseSelectionContext.onSelectionChanged += UpdatePaymentText;
        }

        private void OnDisable()
        {
            houseSelectionContext.onSelectionChanged -= UpdatePaymentText;
        }

        private void UpdatePaymentText()
        {
            if (paymentInputField == null)
            {
                ServiceDebug.LogError("Ссылка на текстовое поле отсутствует");
                return;
            }

            if (!houseSelectionContext.TryGetSelectedData(out HouseData houseData)) return;
            paymentInputField.text = houseData.PaymentPerDay;
        }
    }
}