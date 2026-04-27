using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Обновление контекста выбора <see cref="BaseSelectionContext"/> при изменении выбора в dropdown-списке
    /// </summary>
    [RequireComponent(typeof(DropdownIdBinder))]
    public class SelectionContextUpdateOnDropdown : MonoBehaviour
    {
        [SerializeField] private BaseSelectionContext selectionContext;
        
        private DropdownIdBinder dropdownIdBinder;

        private void Awake()
        {
            dropdownIdBinder = GetComponent<DropdownIdBinder>();
        }

        private void OnEnable()
        {
            if (selectionContext == null) return;

            dropdownIdBinder.onSelectedIdChanged += SelectNewContext;
        }

        private void OnDisable()
        {
            dropdownIdBinder.onSelectedIdChanged -= SelectNewContext;
        }

        private void SelectNewContext(string selectedDataId)
        {
            selectionContext.Select(selectedDataId);
        }
    }
}