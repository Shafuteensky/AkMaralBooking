using Extensions.Generics;
using StarletBooking.Data;

namespace StarletBooking.PhoneContact
{
    /// <summary>
    /// Базовый контроллер доступности кнопки контакта.
    /// Вешается на GameObject кнопки — управляет её собственным <see cref="UnityEngine.UI.Button.interactable"/>
    /// в зависимости от наличия номера у выбранного клиента.
    /// </summary>
    public abstract class PhoneContactAvailabilityBase : AbstractButton
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Subscribe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Unsubscribe();
        }

        /// <summary>Определить доступность кнопки при наличии номера телефона</summary>
        protected abstract bool ResolveAvailability(bool hasNumber);

        private ClientSelectionContext Context => DataBus.Instance.ClientSelectionContext;

        private void Subscribe()
        {
            Context.onSelectionChanged += Refresh;
            Refresh();
        }

        private void Unsubscribe()
        {
            Context.onSelectionChanged -= Refresh;
        }

        private void Refresh()
        {
            ClientData client = Context.HasSelection
                ? Context.GetSelectedData()
                : null;

            bool hasNumber = !string.IsNullOrWhiteSpace(client?.ContactNumber);
            button.interactable = ResolveAvailability(hasNumber);
        }
    }
}
