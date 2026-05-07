using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Двусторонняя синхронизация контекста выбора и dropdown-списка
    /// </summary>
    [RequireComponent(typeof(DropdownIdBinder))]
    public class SelectionContextUpdateOnDropdown : MonoBehaviour
    {
        [SerializeField] private BaseSelectionContext selectionContext;
        
        private DropdownIdBinder dropdownIdBinder;
        private bool isUpdating;
        private bool ignoreNextDropdownSelection;

        private void Awake()
        {
            dropdownIdBinder = GetComponent<DropdownIdBinder>();
        }

        private void OnEnable()
        {
            if (selectionContext == null) return;

            dropdownIdBinder.onSelectedIdChanged += SelectNewContext;
            dropdownIdBinder.onOptionsSet += OnOptionsSet;
            selectionContext.onSelectionChanged += SelectDropdown;

            SelectDropdown();
        }

        private void OnDisable()
        {
            if (dropdownIdBinder != null)
            {
                dropdownIdBinder.onSelectedIdChanged -= SelectNewContext;
                dropdownIdBinder.onOptionsSet -= OnOptionsSet;
            }

            if (selectionContext != null)
                selectionContext.onSelectionChanged -= SelectDropdown;
        }

        private void OnOptionsSet()
        {
            ignoreNextDropdownSelection = true;
            SelectDropdown();
        }
        
        private void SelectNewContext(string selectedDataId)
        {
            if (isUpdating) return;

            if (ignoreNextDropdownSelection)
            {
                ignoreNextDropdownSelection = false;
                return;
            }

            isUpdating = true;

            if (string.IsNullOrEmpty(selectedDataId))
                selectionContext.Clear();
            else
                selectionContext.Select(selectedDataId);

            isUpdating = false;
        }

        private void SelectDropdown()
        {
            if (isUpdating) return;

            isUpdating = true;

            if (selectionContext.HasSelection && selectionContext is ISingleSelectionContext singleSelectionContext)
                dropdownIdBinder.SetSelectedById(singleSelectionContext.SelectedId);
            else
                dropdownIdBinder.ClearSelection();

            isUpdating = false;
        }
    }
}