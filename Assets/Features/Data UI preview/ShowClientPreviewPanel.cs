using Extensions.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Data.Preview
{
    /// <summary>
    /// Вывод информации о клиенте в превью панель списка
    /// </summary>
    public class ShowClientPreviewPanel : ShowPreviewPanel<ClientData>
    {
        [SerializeField]
        protected Image colorImage;
        [SerializeField]
        protected TMP_Text clientName; 
        [SerializeField]
        protected TMP_Text rating;

        protected override void ShowInfo()
        {
            if (Logic.IsNull(clientName) ||
                Logic.IsNull(colorImage) ||
                Logic.IsNull(rating))
            {
                return;
            }

            colorImage.color = data?.Color ?? DataHelpers.NotFoundColor;

            clientName.text = data == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(data.Name); 
            rating.text = data == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(data.Rating.ToString()); 
        }
    }
}