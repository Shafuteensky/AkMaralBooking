using UnityEngine;

namespace Extensions.Data.InMemoryData.SelectionContext
{
    /// <summary>String
    /// Базовый вспомогательный класс контекста выбора InMemoryData-контейнера
    /// </summary>
    public abstract class BaseSelectionContext : ScriptableObject
    {
        /// <summary>
        /// Наличие активного выбранного элемента
        /// </summary>
        public abstract bool HasSelection { get; }
        
        /// <summary>
        /// Идентификатор активной выбранной записи контейнера
        /// </summary>
        public string SelectedId => selectedId;

        protected string selectedId;
        
        /// <summary>
        /// Очистка выбора
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Задание выбора
        /// </summary>
        public abstract void Select(string selectionDataId);
    }
}