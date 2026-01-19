using Extensions.Data.InMemoryData;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Logic;
using UnityEngine;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Базовый загрузчик UI из SelectionContext при открытии страницы
    /// </summary>
    public abstract class SelectionContextViewLoader<TData> : MonoBehaviour where TData : InMemoryDataItem
    {
        [SerializeField]
        protected SelectionContext<TData> selectionContext;

        [SerializeField]
        protected InMemoryDataContainer<TData> container;

        protected virtual void OnEnable()
        {
            if (Logic.IsNull(selectionContext) || Logic.IsNull(container))
            {
                return;
            }

            selectionContext.onSelectionChanged += OnSelectionChanged;
            Rebuild();
        }

        protected virtual void OnDisable()
        {
            if (Logic.IsNull(selectionContext))
            {
                return;
            }

            selectionContext.onSelectionChanged -= OnSelectionChanged;
        }

        /// <summary>
        /// Принудительное заполнение UI из активного выбора
        /// </summary>
        public void Rebuild()
        {
            if (selectionContext == null || container == null)
            {
                return;
            }

            string id = selectionContext.SelectedId;
            if (string.IsNullOrEmpty(id))
            {
                ApplyEmpty();
                return;
            }

            if (!container.TryGetById(id, out TData dataItem) || dataItem == null)
            {
                ApplyEmpty();
                return;
            }

            ApplyToView(dataItem);
        }


        protected void OnSelectionChanged() => Rebuild();
        
        protected abstract void ApplyToView(TData dataItem);

        protected abstract void ApplyEmpty();
    }
}
