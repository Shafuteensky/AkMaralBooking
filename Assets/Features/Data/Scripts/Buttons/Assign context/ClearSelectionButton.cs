using Extensions.Generics;
using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Кнопка очистки выбора <see cref="BaseSelectionContext"/>
    /// </summary>
    public class ClearSelectionButton : AbstractButton
    {
        [SerializeField] private BaseSelectionContext selectionContext; 
        
        public override void OnButtonClick()
        {
            if (Logic.IsNotNull(selectionContext)) selectionContext.Clear();
        }
    }
}