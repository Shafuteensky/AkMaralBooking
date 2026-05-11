using Extensions.Data.InMemoryData;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Generics;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    public abstract class SaveDataButton<TDataContainer, TSelectionContext> : AbstractButtonAction
        where TDataContainer : InMemoryDataBaseObject
        where TSelectionContext : BaseSelectionContext
    {
        [Header("Контейнер данных"), Space]
        [SerializeField] protected TDataContainer dataContainer;
        [SerializeField] protected TSelectionContext selectionContext;
        
        protected string EmptyIfDefault(string value) => 
            value == DataHelpers.NotFoundString || value == DataHelpers.EmptyString ? "" : value;
    }
}