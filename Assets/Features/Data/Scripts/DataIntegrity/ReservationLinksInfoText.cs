using System.Collections.Generic;

namespace StarletBooking.Data
{
    /// <summary>
    /// Отображает информацию о связях записи аренды: привязанный клиент и его другие записи аренды
    /// </summary>
    public sealed class ReservationLinksInfoText : DataLinksInfoText<ReservationLinksChecker>
    {
        protected override string BuildText()
        {
            var parts = new List<string>();

            if (checker.HasLinkedClient)
                parts.Add($"Клиент: {checker.LinkedClient.Name}");

            if (checker.HasOtherClientReservations)
                parts.Add($"Других записей аренды клиента: {checker.OtherClientReservations.Count}");

            return string.Join("\n", parts);
        }
    }
}
