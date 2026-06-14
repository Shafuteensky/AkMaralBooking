using System;
using UnityEngine;

namespace Extensions.Data.InMemoryData.SelectionContext
{
    /// <summary>
    /// Базовый вспомогательный класс контекста выбора InMemoryData-контейнера
    /// </summary>
    public abstract class BaseSelectionContext : ScriptableObject
    {
        /// <summary>
        /// Событие изменения активного элемента
        /// </summary>
        public event Action onSelectionChanged;

        /// <summary>
        /// Наличие активного выбранного элемента
        /// </summary>
        public abstract bool HasSelection { get; }

        /// <summary>
        /// Версия выбора, увеличивается при каждом вызове <see cref="Select"/> или <see cref="Clear"/>
        /// </summary>
        public int SelectionVersion { get; private set; }

        /// <summary>
        /// Очистка выбора
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Задание выбора
        /// </summary>
        public abstract void Select(string selectionDataId);

        protected void OnSelectionChanged()
        {
            SelectionVersion++;
            onSelectionChanged?.Invoke();
        }
    }
}