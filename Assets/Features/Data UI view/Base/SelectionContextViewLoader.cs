using System;
using Extensions.Data.InMemoryData;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Helpers;
using Extensions.Log;
using UnityEngine;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Базовый загрузчик UI из SelectionContext при открытии страницы
    /// </summary>
    public abstract class SelectionContextViewLoader<TData> : MonoBehaviour where TData : InMemoryDataEntry
    {
        [Header("Контейнер данных для вывода"), Space]
        [SerializeField] protected SingleSelectionContext<TData> singleSelectionContext;
        
        protected InMemoryDataContainer<TData> container;

        protected void Awake()
        {
            if (Logic.IsNull(singleSelectionContext))
            {
                return;
            }
            
            container = singleSelectionContext.Container;
        }

        protected virtual void OnEnable()
        {
            if (Logic.IsNull(singleSelectionContext) || Logic.IsNull(container))
            {
                return;
            }

            singleSelectionContext.onSelectionChanged += OnSingleSelectionChanged;
            Rebuild();
        }

        protected virtual void OnDisable()
        {
            if (Logic.IsNull(singleSelectionContext))
            {
                return;
            }

            singleSelectionContext.onSelectionChanged -= OnSingleSelectionChanged;
        }

        /// <summary>
        /// Принудительное заполнение UI из активного выбора
        /// </summary>
        public void Rebuild()
        {
            ApplyEmpty();
            
            if (singleSelectionContext == null || container == null) return;

            string id = singleSelectionContext.SelectedId;
            
            if (string.IsNullOrEmpty(id)) return;
            
            if (!container.GetById(id, out TData dataItem) || dataItem == null)
            {
                ServiceDebug.LogError("Ошибка получения данных");
                return;
            }

            ApplyToView(dataItem);
        }

        protected void OnSingleSelectionChanged() => Rebuild();
        
        protected abstract void ApplyToView(TData dataItem);

        protected abstract void ApplyEmpty();
    }
}
