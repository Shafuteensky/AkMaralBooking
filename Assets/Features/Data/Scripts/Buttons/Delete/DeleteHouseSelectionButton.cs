using Extensions.Data.InMemoryData.SelectionContext;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Удаление данных о доме
    /// </summary>
    public class DeleteHouseSelectionButton : DeleteSelectionContextButtonBase<HouseData> 
    {
        protected override SingleSelectionContext<HouseData> singleSelectionContext => DataBus.Instance.HouseSelectionContext;
    }
}