using Extensions.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.ColorPicker
{
    /// <summary>
    /// Обновление цвета изображения при измненеии выбора ColorPicker
    /// </summary>
    public class ColorPickerListener : MonoBehaviour
    {
        [SerializeField]
        protected ColorPicker colorPicker;
        [SerializeField]
        protected Image image;

        protected virtual void OnEnable()
        {
            if (Logic.IsNotNull(colorPicker))
            {
                colorPicker.OnColorChange?.AddListener(UpdateImageColor);
            }

            UpdateImageColor(colorPicker.GetColor());
        }
        
        protected virtual void OnDisable()
        {
            if (Logic.IsNotNull(colorPicker))
            {
                colorPicker.OnColorChange?.RemoveListener(UpdateImageColor);
            }
        }

        protected void UpdateImageColor(Color newColor)
        {
            if (Logic.IsNotNull(image) && Logic.IsNotNull(colorPicker))
            {
                image.color = colorPicker.GetColor();
            }
        }
    }
}