using System;
using Extensions.Data.InMemoryData;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Базовый класс проверки связности выбранной записи с другими данными
    /// </summary>
    public abstract class DataLinksChecker : MonoBehaviour
    {
        /// <summary>Событие завершения проверки связей</summary>
        public event Action onLinksChecked;

        /// <summary>Есть ли связанные записи</summary>
        public abstract bool HasLinks { get; }

        protected void NotifyChecked() => onLinksChecked?.Invoke();
    }

    /// <summary>
    /// Базовый класс проверки связности выбранной записи с привязкой к SelectionContext
    /// </summary>
    public abstract class DataLinksChecker<TData> : DataLinksChecker
        where TData : InMemoryDataEntry
    {
        [SerializeField] private SingleSelectionContext<TData> selectionContext;

        protected virtual void OnEnable()
        {
            if (Logic.IsNull(selectionContext, nameof(selectionContext))) return;
            if (!selectionContext.TryGetSelectedData(out TData data)) return;
            PerformCheck(data);
            NotifyChecked();
        }

        /// <summary>Выполнить проверку связей для указанной записи</summary>
        protected abstract void PerformCheck(TData data);
    }
}
