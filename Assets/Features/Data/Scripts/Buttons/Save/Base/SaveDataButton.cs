using Extensions.Data.InMemoryData;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Generics;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    public abstract class SaveDataButton<TDataContainer, TSelectionContext> : AbstractButton
        where TDataContainer : InMemoryDataBaseObject
        where TSelectionContext : BaseSelectionContext
    {
        [Header("Контейнер данных"), Space]
        [SerializeField] protected TDataContainer dataContainer;
        [SerializeField] protected TSelectionContext selectionContext;
    }
}