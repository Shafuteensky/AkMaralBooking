using Extensions.Generics;
using StarletBooking.Data;
using TMPro;
using UnityEngine;

namespace StarletBooking.PhoneContact
{
    /// <summary>
    /// Базовый контроллер доступности кнопки контакта.
    /// Вешается на GameObject кнопки — управляет её собственным <see cref="UnityEngine.UI.Button.interactable"/>
    /// в зависимости от наличия номера у выбранного клиента.
    /// </summary>
    public abstract class PhoneContactAvailabilityBase : AbstractButton
    {
        [SerializeField] private TMP_InputField phoneNumber;
        
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

        private void Subscribe()
        {
            phoneNumber.onValueChanged.AddListener(Refresh);
            Refresh(phoneNumber.text);
        }

        private void Unsubscribe()
        {
            phoneNumber.onValueChanged.RemoveListener(Refresh);
        }

        private void Refresh(string number)
        {
            bool hasNumber = !string.IsNullOrEmpty(number);
            button.interactable = ResolveAvailability(hasNumber);
        }
    }
}
