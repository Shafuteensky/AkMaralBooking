using Extensions.Logic;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Preview
{
    /// <summary>
    /// Вывод информации о доме в превью панель списка
    /// </summary>
    public class ShowHousePreviewPanel : ShowPreviewPanel<HouseData>
    {
        [SerializeField]
        protected TMP_Text number; 
        [SerializeField]
        protected TMP_Text houseName;

        protected override void ShowInfo()
        {
            if (Logic.IsNull(number) ||
                Logic.IsNull(houseName))
            {
                return;
            }

            number.text = data.Number;
            houseName.text = data.Name;
        }
    }
}