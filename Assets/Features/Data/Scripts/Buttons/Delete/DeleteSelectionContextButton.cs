using Extensions.Generics;
using Extensions.Log;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Data.InMemoryData;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Универсальная кнопка удаления активного элемента из SelectionContext
    /// </summary>
    public abstract class DeleteSelectionContextButtonBase<TData> : AbstractButtonAction
        where TData : InMemoryDataEntry
    {
        [SerializeField]
        protected SelectionContext<TData> selectionContext;

        public override void OnButtonClickAction()
        {
            if (Logic.IsNull(selectionContext) ||
                Logic.IsNull(selectionContext.Container) ||
                !selectionContext.HasSelection)
            { return; }

            string id = selectionContext.SelectedId;
            if (string.IsNullOrEmpty(id))
            {
                ServiceDebug.LogWarning("Идентификатор активного элемента пуст, удаление невозможно");
                return;
            }

            selectionContext.Container.Remove(id);
            selectionContext.Clear();
        }
    }
}