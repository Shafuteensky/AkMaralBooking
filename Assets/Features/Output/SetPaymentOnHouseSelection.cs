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
        private int lastSyncedSelectionVersion = -1;

        protected override void Awake()
        {
            base.Awake();
            houseSelectionContext = DataBus.Instance.HouseSelectionContext;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            houseSelectionContext.onSelectionChanged += UpdatePaymentText;

            if (houseSelectionContext.SelectionVersion != lastSyncedSelectionVersion)
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

            lastSyncedSelectionVersion = houseSelectionContext.SelectionVersion;

            if (!houseSelectionContext.TryGetSelectedData(out HouseData houseData))
                inputField.text = Formatters.FormatFloat(DEFAULT_PAYMENT, 3);
            else
                inputField.text = houseData.PaymentPerDay;
        }
    }
}