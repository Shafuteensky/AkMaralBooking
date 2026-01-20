using StarletBooking.Data;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Заполнение фабрикой списка домов по завершению сохранения
    /// </summary>
    public class PopulateHousesOnAfterSave : PopulateOnAfterSave<HouseData> { }
}