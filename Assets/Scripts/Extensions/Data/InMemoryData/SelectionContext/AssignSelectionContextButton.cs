using Extensions.Generics;
using Extensions.Log;

namespace Extensions.Data.InMemoryData.SelectionContext
{
    /// <summary>
    /// Кнопка
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class AssignSelectionContextButton<TData> : GenericButton
        where TData : InMemoryDataItem
    {
        private SelectionContext<TData> selectionContext;
        private InMemoryDataContainer<TData> container;
        private string entryId;

        public void Initialize(SelectionContext<TData> context, InMemoryDataContainer<TData> sourceContainer, string id)
        {
            selectionContext = context;
            container = sourceContainer;
            entryId = id;
        }
        
        public override void OnButtonClick()
        {
            if (selectionContext == null || container == null || string.IsNullOrEmpty(entryId))
            {
                ServiceDebug.LogError("Инициализация не выполнена или не полностью выполнена");
                return;
            }

            selectionContext.Select(container, entryId);
        }
    }
}
