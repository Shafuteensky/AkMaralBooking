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
        protected abstract SingleSelectionContext<TData> singleSelectionContext { get; }

        public override void OnButtonClickAction()
        {
            if (Logic.IsNull(singleSelectionContext) ||
                Logic.IsNull(singleSelectionContext.Container) ||
                !singleSelectionContext.HasSelection)
            { return; }

            string id = singleSelectionContext.SelectedId;
            if (string.IsNullOrEmpty(id))
            {
                ServiceDebug.LogWarning("Идентификатор активного элемента пуст, удаление невозможно");
                return;
            }

            singleSelectionContext.Container.Remove(id);
            singleSelectionContext.Clear();
        }
    }
}