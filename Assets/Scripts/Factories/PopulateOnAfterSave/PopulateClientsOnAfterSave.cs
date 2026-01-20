using StarletBooking.Data;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Заполнение фабрикой списка клиентов по завершению сохранения
    /// </summary>
    public class PopulateClientsOnAfterSave : PopulateOnAfterSave<ClientData> { }
}