using Extensions.Data.InMemoryData.SelectionContext;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Удаление данных о клиенте
    /// </summary>
    public class DeleteClientSelectionButton : DeleteSelectionContextButtonBase<ClientData>
    {
        protected override SingleSelectionContext<ClientData> singleSelectionContext => DataBus.Instance.ClientSelectionContext;
    }
}