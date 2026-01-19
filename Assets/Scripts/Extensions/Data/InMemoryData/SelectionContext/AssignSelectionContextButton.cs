using System;
using Extensions.Generics;
using Extensions.Log;
using UnityEngine;

namespace Extensions.Data.InMemoryData.SelectionContext
{
    /// <summary>
    /// КНопка назначения контекста данных
    /// </summary>
    /// <typeparam name="TData">Тип данных</typeparam>
    public abstract class AssignSelectionContextButton<TData> : GenericButton
        where TData : InMemoryDataItem
    {
        [SerializeField]
        private SelectionContext<TData> selectionContext;
        [SerializeField]
        private InMemoryDataContainer<TData> container;
        
        private string _entryId;

        /// <summary>
        /// Инициализация данных
        /// </summary>
        /// <param name="id">Идентификатор записи данных</param>
        public void Initialize(string id) => _entryId = id;
        
        public override void OnButtonClick()
        {
            if (selectionContext == null || container == null || string.IsNullOrEmpty(_entryId))
            {
                ServiceDebug.LogError("Инициализация не выполнена или не полностью выполнена");
                return;
            }

            selectionContext.Select(container, _entryId);
        }
    }
}
