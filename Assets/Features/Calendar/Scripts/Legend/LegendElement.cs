using Extensions.Data.InMemoryData.SelectionContext;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Элемент легенды календаря
    /// </summary>
    public class LegendElement : ContextIdHolder
    {
        [Header("Элементы"), Space]
        [SerializeField] private Image colorImage;
        [SerializeField] private TMP_Text text;
        
        [Header("Материалы"), Space]
        [SerializeField] private Sprite houseImage;
        [SerializeField] private Sprite clientImage;
        
        /// <summary>
        /// Установить цвет элемента
        /// </summary>
        public void SetColor(Color color) => colorImage.color = color;
        
        /// <summary>
        /// Установить текст элемента
        /// </summary>
        public void SetText(string value) => text.text = value;
        
        /// <summary>
        /// Установить иконку клиента
        /// </summary>
        public void SetAsClient() => colorImage.sprite = clientImage;
        
        /// <summary>
        /// Установить иконку дома
        /// </summary>
        public void SetAsHouse() => colorImage.sprite = houseImage;
    }
}