using UnityEngine;
using UnityEngine.UI;

namespace EZCalendarWeeklyView
{
    /// <summary>
    /// Dynamically adjusts the GridLayoutGroup based on the available space.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class DynamicGridLayout : MonoBehaviour
    {
        public float cellPadding = 5f;  // Padding between cells
        public float minCellSize = 50f; // Minimum size for each cell

        protected RectTransform rectTransform;
        protected GridLayoutGroup gridLayout;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            gridLayout = GetComponent<GridLayoutGroup>();
        }

        /// <summary>
        /// Updates the layout of the grid based on the available space.
        /// </summary>
        public void UpdateGridLayout()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            if (gridLayout == null) gridLayout = GetComponent<GridLayoutGroup>();
            
            // Calculate available width and height
            float availableWidth = rectTransform.rect.width;
            float availableHeight = rectTransform.rect.height;

            // Define number of columns and rows
            int columns = 7; // Maximum of 7 days in a week
            int rows = 6;    // Maximum of 6 weeks in a month

            // Calculate cell size and spacing dynamically
            float cellSizeX = (availableWidth - (columns - 1) * cellPadding) / columns;
            float cellSizeY = (availableHeight - (rows - 1) * cellPadding) / rows;

            // Update GridLayoutGroup properties
            gridLayout.cellSize = new Vector2(cellSizeX, cellSizeY);
            gridLayout.spacing = new Vector2(cellPadding, cellPadding);
        }

        protected virtual void Update()
        {
            // Uncomment the following line if you want to update dynamically (e.g., on parent size change)
            // UpdateGridLayout();
        }
    }
}