using Extensions.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Data.Preview
{
    /// <summary>
    /// Вывод информации о доме в превью панель списка
    /// </summary>
    public class ShowHousePreviewPanel : ShowPreviewPanel<HouseData>
    {
        [SerializeField]
        protected Image colorImage;
        [SerializeField]
        protected TMP_Text number; 
        [SerializeField]
        protected TMP_Text houseName;

        protected override void ShowInfo()
        {
            if (Logic.IsNull(number) ||
                Logic.IsNull(colorImage) ||
                Logic.IsNull(houseName))
            {
                return;
            }
            
            colorImage.color = data?.Color ?? DataHelpers.NotFoundColor;

            number.text = data == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(data.Number); 
            houseName.text = data == null ? DataHelpers.NotFoundString : 
                DataHelpers.GetString(data.Name); 
        }
    }
}