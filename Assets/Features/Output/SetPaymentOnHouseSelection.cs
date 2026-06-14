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
        private string lastAppliedHouseId;
        private bool initialized;

        protected override void Awake()
        {
            base.Awake();
            houseSelectionContext = DataBus.Instance.HouseSelectionContext;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            houseSelectionContext.onSelectionChanged += UpdatePaymentText;

            string currentHouseId = houseSelectionContext.TryGetSelectedData(out HouseData houseData) ? houseData.Id : string.Empty;
            if (!initialized || currentHouseId != lastAppliedHouseId)
                UpdatePaymentText();
        }

        protected override  void OnDisable()
        {
            base.OnDisable();
            houseSelectionContext.onSelectionChanged -= UpdatePaymentText;
        }

        private void UpdatePaymentText()
        {
            if (Logic.IsNull(houseSelectionContext)) return;

            initialized = true;

            if (!houseSelectionContext.TryGetSelectedData(out HouseData houseData))
            {
                inputField.text = Formatters.FormatFloat(DEFAULT_PAYMENT, 3);
                lastAppliedHouseId = string.Empty;
            }
            else
            {
                inputField.text = houseData.PaymentPerDay;
                lastAppliedHouseId = houseData.Id;
            }
        }
    }
}