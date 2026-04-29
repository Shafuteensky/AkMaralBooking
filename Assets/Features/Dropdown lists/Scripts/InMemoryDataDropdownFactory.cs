using System.Collections.Generic;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Фабрика заполнения dropdown-списка из InMemoryData-контейнера
    /// </summary>
    [RequireComponent(typeof(DropdownIdBinder))]
    public sealed class InMemoryDataDropdownFactory : MonoBehaviour
    {
        [SerializeField]
        private InMemoryDropdownOptionsProvider optionsProvider;

        [SerializeField]
        private bool rebuildOnEnable = true;
        
        private DropdownIdBinder binder;

        private readonly List<string> labels = new List<string>();
        private readonly List<string> ids = new List<string>();
        
        private void Awake() => binder = GetComponent<DropdownIdBinder>();

        private void OnEnable()
        {
            if (rebuildOnEnable)
            {
                Rebuild();
            }
        }

        /// <summary>
        /// Перезаполнение dropdown-списка
        /// </summary>
        public void Rebuild()
        {
            if (Logic.IsNull(optionsProvider) || 
                Logic.IsNull(binder))
            {
                return;
            }
            
            optionsProvider.BuildOptions(labels, ids);
            binder.SetOptions(labels, ids);
        }

        /// <summary>
        /// Получить идентификатор выбранного элемента
        /// </summary>
        public string GetSelectedId()
        {
            if (binder == null)
            {
                return string.Empty;
            }

            return binder.SelectedId;
        }

        /// <summary>
        /// Назначить актвный элемент по идентификатору
        /// </summary>
        public void SetSelectedById(string id)
        {
            if (binder == null)
            {
                return;
            }

            binder.SetSelectedById(id);
        }
    }
}