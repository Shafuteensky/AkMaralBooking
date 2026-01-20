using Extensions.Logic;
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

            colorImage.color = data.Color;
            clientName.text = data.Name;
            rating.text = data.Rating.ToString();
        }
    }
}