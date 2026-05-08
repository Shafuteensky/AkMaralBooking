using Extensions.Generics;
using Extensions.Helpers;
using StarletBooking.Data;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Вывод стоимости аренды дома при изменении контекста выбора
    /// </summary>
    public sealed class SetPaymentOnHouseSelection : AbstractInputField
    {
        private const float DEFAULT_PAYMENT = 0;
        
        private HouseSelectionContext houseSelectionContext;

        protected override void Awake()
        {
            base.Awake();
            houseSelectionContext = DataBus.Instance.HouseSelectionContext;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            houseSelectionContext.onSelectionChanged += UpdatePaymentText;
        }

        protected override  void OnDisable()
        {
            base.OnDisable();
            houseSelectionContext.onSelectionChanged -= UpdatePaymentText;
        }

        private void UpdatePaymentText()
        {
            if (Logic.IsNull(houseSelectionContext)) return;
            if (!houseSelectionContext.TryGetSelectedData(out HouseData houseData))
                inputField.text = Formatters.FormatFloat(DEFAULT_PAYMENT);
            else
                inputField.text = houseData.PaymentPerDay;
        }
    }
}