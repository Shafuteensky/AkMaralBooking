namespace StarletBooking.PhoneContact
{
    /// <summary>
    /// Делает кнопку звонка доступной если у выбранного клиента есть номер телефона.
    /// </summary>
    public sealed class CallContactAvailabilityController : PhoneContactAvailabilityBase
    {
        protected override bool ResolveAvailability(bool hasNumber) => hasNumber;
    }
}
