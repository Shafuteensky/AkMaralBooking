using UnityEngine;
using UnityEngine.UI;

namespace Extensions.Layouts
{
    /// <summary>
    /// Автоматическая подстройка <see cref="GridLayoutGroup"/> под свободное пространство
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class DynamicGridLayout : MonoBehaviour
    {
        [Header("Сетка"), Space]
        [Tooltip("Количество колонн"), Min(1)]
        [SerializeField] private float columns = 7;
        [Tooltip("Количество рядов"), Min(1)]
        [SerializeField] private float rows = 6;
        
        [Header("Ячейки"), Space]
        [Tooltip("Расстояние между ячейками")]
        [SerializeField] private float cellPadding = 5f;
        // [Tooltip("Минимальный размер каждой отдельной ячейки")]
        // [SerializeField] private  float minCellSize = 50f;
        
        [Header("Макет"), Space]
        [Tooltip("Автообновление макета каждый кадр"), Min(1)]
        [SerializeField] private bool updateDynamically = false;

        private RectTransform rectTransform;
        private GridLayoutGroup gridLayout;

        /// <summary>
        /// Автоматическая подстройка макета под свободное пространство
        /// </summary>
        public void UpdateGridLayout()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            if (gridLayout == null) gridLayout = GetComponent<GridLayoutGroup>();
            
            float availableWidth = rectTransform.rect.width;
            float availableHeight = rectTransform.rect.height;

            float cellSizeX = (availableWidth - (columns - 1) * cellPadding) / columns;
            float cellSizeY = (availableHeight - (rows - 1) * cellPadding) / rows;

            gridLayout.cellSize = new Vector2(cellSizeX, cellSizeY);
            gridLayout.spacing = new Vector2(cellPadding, cellPadding);
        }

        private void Update()
        {
            if (updateDynamically) UpdateGridLayout();
        }
    }
}