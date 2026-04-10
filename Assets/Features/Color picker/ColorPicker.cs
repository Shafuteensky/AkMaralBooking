using UnityEngine;

namespace StarletBooking.ColorPicker
{
    /// <summary>
    /// Панель выбора цвета
    /// </summary>
    public class ColorPicker : ColorPaletteController
    {
        /// <summary>
        /// Получение активного цвета
        /// </summary>
        /// <param name="color"></param>
        public Color GetColor() => SelectedColor;
        
        /// <summary>
        /// Установка активного цвета
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color) => SelectedColor = color;
    }
}