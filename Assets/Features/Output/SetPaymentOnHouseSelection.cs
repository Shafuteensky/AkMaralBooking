using Extensions.Generics;
using Extensions.Helpers;
using StarletBooking.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Вывод стоимости аренды дома при изменении контекста выбора
    /// </summary>
    public class SetPaymentOnHouseSelection : AbstractInputField
    {
        private const float DEFAULT_PAYMENT = 0;
        
        [FormerlySerializedAs("houseSelectionContext")] [SerializeField] protected HouseSingleSelectionContext houseSingleSelectionContext;

        protected override void OnEnable()
        {
            base.OnEnable();
            houseSingleSelectionContext.onSelectionChanged += UpdatePaymentText;
        }

        protected override  void OnDisable()
        {
            base.OnDisable();
            houseSingleSelectionContext.onSelectionChanged -= UpdatePaymentText;
        }

        private void UpdatePaymentText()
        {
            if (Logic.IsNull(houseSingleSelectionContext)) return;
            if (!houseSingleSelectionContext.TryGetSelectedData(out HouseData houseData))
                inputField.text = Formatters.FormatFloat(DEFAULT_PAYMENT);
            else
                inputField.text = houseData.PaymentPerDay;
        }
    }
}