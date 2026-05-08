using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Обновление активности элемента по состоянию выбора в контексте выбора <see cref="BaseSelectionContext"/>
    /// </summary>
    public sealed class ElementActivityBySelection : MonoBehaviour
    {
        [SerializeField] private GameObject element;
        [SerializeField] private BaseSelectionContext selectionContext;

        private void OnEnable()
        {
            if (Logic.IsNotNull(selectionContext)) 
                selectionContext.onSelectionChanged += RefreshActivity;

            RefreshActivity();
        }

        private void OnDisable()
        {
            if (Logic.IsNotNull(selectionContext)) 
                selectionContext.onSelectionChanged -= RefreshActivity;
        }

        private void RefreshActivity()
        {
            if (selectionContext != null && element != null)
                element.SetActive(selectionContext.HasSelection);
        }
    }
}