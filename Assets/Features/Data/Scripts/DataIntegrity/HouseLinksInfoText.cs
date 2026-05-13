namespace StarletBooking.Data
{
    /// <summary>
    /// Отображает количество записей аренды, привязанных к дому
    /// </summary>
    public sealed class HouseLinksInfoText : DataLinksInfoText<HouseLinksChecker>
    {
        protected override string BuildText() =>
            $"Связанных записей аренды: {checker.LinkedReservations.Count}";
    }
}
