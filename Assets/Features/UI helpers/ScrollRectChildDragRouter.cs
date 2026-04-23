using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    /// <summary>
    /// Маршрутизирует drag- и scroll-события с дочернего UI-элемента
    /// в родительский ScrollRect, чтобы свайп по интерактивному элементу
    /// не блокировал прокрутку списка.
    /// </summary>
    public sealed class ScrollRectChildDragRouter : MonoBehaviour,
        IInitializePotentialDragHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IScrollHandler
    {
        private ScrollRect parentScrollRect;
        private bool isRoutingDrag;

        private void Awake()
        {
            parentScrollRect = GetComponentInParent<ScrollRect>();

            if (parentScrollRect == null) enabled = false;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (parentScrollRect == null) return;

            parentScrollRect.OnInitializePotentialDrag(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (parentScrollRect == null) return;

            if (!ShouldRouteDragToScroll(eventData))
            {
                isRoutingDrag = false;
                return;
            }

            isRoutingDrag = true;
            parentScrollRect.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isRoutingDrag || parentScrollRect == null) return;

            parentScrollRect.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isRoutingDrag && parentScrollRect != null) parentScrollRect.OnEndDrag(eventData);

            isRoutingDrag = false;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (parentScrollRect == null) return;

            parentScrollRect.OnScroll(eventData);
        }

        private bool ShouldRouteDragToScroll(PointerEventData eventData)
        {
            Vector2 delta = eventData.delta;

            if (parentScrollRect.vertical && !parentScrollRect.horizontal)
            {
                return Mathf.Abs(delta.y) >= Mathf.Abs(delta.x);
            }

            if (!parentScrollRect.vertical && parentScrollRect.horizontal)
            {
                return Mathf.Abs(delta.x) >= Mathf.Abs(delta.y);
            }

            if (parentScrollRect.vertical && parentScrollRect.horizontal)
            {
                return delta.sqrMagnitude > 0f;
            }

            return false;
        }
    }
}