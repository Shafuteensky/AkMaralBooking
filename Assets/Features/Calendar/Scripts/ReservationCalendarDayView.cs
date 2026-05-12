using System.Collections.Generic;
using Extensions.Generics;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Отображает цветовые полоски записей аренды на кнопке дня календаря
    /// </summary>
    public sealed class ReservationCalendarDayView : MonoBehaviour
    {
        [SerializeField] private Transform stripsRoot;
        [SerializeField] private ObservableImage stripPrefab;
        [SerializeField] private GameObject frame;

        private readonly List<ObservableImage> strips = new List<ObservableImage>();

        /// <summary>
        /// Установить цвета записей аренды
        /// </summary>
        public void SetColors(IReadOnlyList<Color> colors)
        {
            if (colors == null || colors.Count == 0)
            {
                Clear();
                return;
            }

            EnsureStripsCount(colors.Count);

            for (int i = 0; i < strips.Count; i++)
            {
                bool isActive = i < colors.Count;
                strips[i].gameObject.SetActive(isActive);

                if (isActive) strips[i].SetColor(colors[i]);
            }
        }

        /// <summary>
        /// Очистить цвета записей аренды
        /// </summary>
        public void Clear()
        {
            foreach (var t in strips)
            {
                t.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Установить активность выделения (рамки)
        /// </summary>
        public void SetFrame(bool isActive) => frame.SetActive(isActive);

        private void EnsureStripsCount(int count)
        {
            if (stripsRoot == null || stripPrefab == null) return;

            while (strips.Count < count)
            {
                ObservableImage strip = Instantiate(stripPrefab, stripsRoot);
                strips.Add(strip);
            }
        }
    }
}