using Extensions.Log;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Extensions.Data.InMemoryData.SelectionContext
{
    /// <summary>
    /// Кнопка назначения контекста данных
    /// </summary>
    /// <typeparam name="TData">Тип данных</typeparam>
    [RequireComponent(typeof(ContextIdHolder))]
    public sealed class AssignSelectionContextButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private BaseSelectionContext selectionContext;

        private ContextIdHolder idHolder;

        private void Awake() => idHolder = GetComponent<ContextIdHolder>();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (selectionContext == null || !idHolder.IsInitialized)
            {
                ServiceDebug.LogError("Инициализация не выполнена или не полностью выполнена");
                return;
            }

            selectionContext.Select(idHolder.EntryId);
        }
    }
}