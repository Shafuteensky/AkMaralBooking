using System;
using Extensions.Data.InMemoryData;
using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data.Preview
{
    /// <summary>
    /// Вывод информации в превью панель списка
    /// </summary>
    [RequireComponent(typeof(ContextIdHolder))]
    public abstract class ShowPreviewPanel<TData> : MonoBehaviour
        where TData : InMemoryDataEntry
    {
        protected TData data => container.GetById(idHolder.EntryId);
        
        [SerializeField]
        private InMemoryDataContainer<TData> container;
        
        private ContextIdHolder idHolder;
        
        protected virtual void Awake() => idHolder = GetComponent<ContextIdHolder>();

        protected virtual void OnEnable()
        {
            if (idHolder.IsInitialized)
            {
                ShowInfo();
            }
            else
            {
                idHolder.onInitialized += ShowInfo;
            }
        }

        protected virtual void OnDisable() => idHolder.onInitialized -= ShowInfo;

        /// <summary>
        /// Вывод информации
        /// </summary>
        protected abstract void ShowInfo();
    }
}