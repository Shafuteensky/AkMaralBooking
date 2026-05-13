namespace StarletBooking.Data
{
    /// <summary>
    /// Отображает количество записей аренды, привязанных к клиенту
    /// </summary>
    public sealed class ClientLinksInfoText : DataLinksInfoText<ClientLinksChecker>
    {
        protected override string BuildText() =>
            $"Связанных записей аренды: {checker.LinkedReservations.Count}";
    }
}
