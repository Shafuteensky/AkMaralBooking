using Extensions.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace StarletBooking.Data.Preview
{
    /// <summary>
    /// Вывод информации о записи аренды в превью панель списка
    /// </summary>
    public class ShowReservationPreviewPanel : ShowPreviewPanel<ReservationData>
    {
        [SerializeField] protected Image colorImage;
        
        [Header("Клиент")]
        [SerializeField] protected ClientsDataContainer clientsContainer;
        [SerializeField] protected TMP_Text clientName; 
        
        [Header("Дом")]
        [SerializeField] protected HousesDataContainer housesContainer;
        [FormerlySerializedAs("houseSingleSelectionContext")] [SerializeField] protected HouseSelectionContext houseSelectionContext;
        [SerializeField] protected TMP_Text houseNumber; 
        
        [Header("Даты")]
        [SerializeField] protected TMP_Text arrivalDate;
        [SerializeField] protected TMP_Text departureDate;

        protected override void ShowInfo()
        {
            if (Logic.IsNull(clientName) ||
                Logic.IsNull(colorImage) ||
                Logic.IsNull(clientsContainer) ||
                Logic.IsNull(houseNumber) ||
                Logic.IsNull(housesContainer) ||
                Logic.IsNull(houseSelectionContext) ||
                Logic.IsNull(arrivalDate) ||
                Logic.IsNull(departureDate))
            {
                return;
            }
            
            ClientData clientData = clientsContainer.GetById(data.ClientId);
            clientName.text = clientData == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(clientData.Name);
            
            HouseData houseData = housesContainer.GetById(data.HouseId);
            houseNumber.text = houseData == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(houseData.Number); 
            
            arrivalDate.text = data == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(data.ArrivalDate.ToShortDateString()); 
            departureDate.text = data == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(data.DepartureDate.ToShortDateString()); 
            
            if (houseSelectionContext.HasSelection)
                colorImage.color = clientData?.Color ?? DataHelpers.NotFoundColor; 
            else
                colorImage.color = houseData?.Color ?? DataHelpers.NotFoundColor; 
        }
    }
}