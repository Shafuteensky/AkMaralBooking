using System.Collections.Generic;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Фабрика заполнения dropdown-списка из InMemoryData-контейнера
    /// </summary>
    [RequireComponent(typeof(DropdownIdBinder))]
    public class InMemoryDataDropdownFactory : MonoBehaviour
    {
        [SerializeField]
        protected InMemoryDropdownOptionsProvider optionsProvider;

        [SerializeField]
        protected bool rebuildOnEnable = true;
        
        protected DropdownIdBinder binder;

        protected readonly List<string> labels = new List<string>();
        protected readonly List<string> ids = new List<string>();

        protected void Awake() => binder = GetComponent<DropdownIdBinder>();

        protected virtual void OnEnable()
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
            binder.SetOptions(labels, ids, 0);
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