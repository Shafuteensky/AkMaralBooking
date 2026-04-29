using DG.Tweening;
using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Проигрывает DOTweenAnimation при изменении SelectionContext
    /// </summary>
    [RequireComponent(typeof(DOTweenAnimation))]
    public sealed class PlayTweenOnSelectionChanged : MonoBehaviour
    {
        [SerializeField] private BaseSelectionContext selectionContext;

        private DOTweenAnimation tweenAnimation;

        private void Awake()
        {
            tweenAnimation = GetComponent<DOTweenAnimation>();
        }

        private void OnEnable()
        {
            if (selectionContext != null)
                selectionContext.onSelectionChanged += OnSelectionChanged;
        }

        private void OnDisable()
        {
            if (selectionContext != null)
                selectionContext.onSelectionChanged -= OnSelectionChanged;
        }

        /// <summary>
        /// Вызывается при изменении выбранного элемента
        /// </summary>
        private void OnSelectionChanged()
        {
            if (tweenAnimation == null) return;

            tweenAnimation.DORestart();
        }
    }
}