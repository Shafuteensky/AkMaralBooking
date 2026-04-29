using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Базовый селектор выбора выпадающего списка 
    /// </summary>
    [RequireComponent(typeof(DropdownIdBinder))]
    public abstract class DropdownOptionSelector : MonoBehaviour
    {
        [SerializeField] protected BaseSelectionContext selectionContext;

        protected DropdownIdBinder binder;
        
        protected virtual void Awake()
        {
            binder = GetComponent<DropdownIdBinder>();
            if (binder != null) binder.onOptionsSet += Select;
        }

        private void OnEnable()
        {
            binder.ClearSelection();
            Select();
        }

        protected virtual void OnDestroy()
        {
            if (binder != null) binder.onOptionsSet -= Select;
        }

        protected abstract void Select();
    }
}