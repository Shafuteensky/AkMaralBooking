using Extensions.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Data.Preview
{
    /// <summary>
    /// Вывод информации о записи аренды в превью панель списка
    /// </summary>
    public class ShowReservationPreviewPanel : ShowPreviewPanel<ReservationData>
    {
        [Header("Клиент")]
        [SerializeField]
        protected Image colorImage;
        [SerializeField]
        protected ClientsDataContainer clientsContainer;
        [SerializeField]
        protected TMP_Text clientName; 
        
        [Header("Дом")]
        [SerializeField]
        protected HousesDataContainer housesContainer;
        [SerializeField]
        protected TMP_Text houseNumber; 
        
        [Header("Даты")]
        [SerializeField]
        protected TMP_Text arrivalDate;
        [SerializeField]
        protected TMP_Text departureDate;

        protected override void ShowInfo()
        {
            if (Logic.IsNull(clientName) ||
                Logic.IsNull(clientName) ||
                Logic.IsNull(clientsContainer) ||
                Logic.IsNull(houseNumber) ||
                Logic.IsNull(housesContainer) ||
                Logic.IsNull(arrivalDate) ||
                Logic.IsNull(departureDate))
            {
                return;
            }

            ClientData clientData = clientsContainer.GetById(data.ClientId);
            colorImage.color = clientData.Color;
            clientName.text = clientData.Name;
            
            houseNumber.text = housesContainer.GetById(data.HouseId).Number;
            
            arrivalDate.text = data.ArrivalDate.ToShortDateString();
            departureDate.text = data.DepartureDate.ToShortDateString();
        }
    }
}