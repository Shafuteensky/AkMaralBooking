using DG.Tweening;
using Extensions.Generics;
using Extensions.ScriptableValues;
using Project.UI;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Data.View
{
    /// <summary>
    /// Кнопка для сворачивания/разворачивания панели
    /// </summary>
    public class PanelCollapseButton : AbstractButtonAction
    {
        [Min(0)]
        [SerializeField] private float collapsedHeight = 50f;

        [SerializeField] private ContentSizeFitter fitter;
        [SerializeField] private RectTransform panel;

        [SerializeField] private DOTweenAnimation collapseAnimation;
        [SerializeField] private BoolValue collapseState;

        private ScrollRectChildDragRouter dragRouter;
        private bool isCollapsed;

        protected override void Awake()
        {
            base.Awake();
            dragRouter = GetComponent<ScrollRectChildDragRouter>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (collapseState != null)
            {
                isCollapsed = collapseState.Value;
                SwitchCollapse(isCollapsed);
            }
        }

        public override void OnButtonClickAction()
        {
            if (dragRouter != null && dragRouter.WasDragged) return;

            isCollapsed = !isCollapsed;
            SwitchCollapse(isCollapsed);
        }

        private void SwitchCollapse(bool isCollapsed)
        {
            if (fitter != null) fitter.enabled = !isCollapsed;

            if (isCollapsed && panel != null)
            {
                panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, collapsedHeight);
                if (collapseAnimation != null) collapseAnimation.tween.Restart();
            }
            else if (collapseAnimation != null)
            {
                collapseAnimation.tween.PlayBackwards();
            }

            if (collapseState != null) collapseState.SetValue(isCollapsed);
            LayoutRebuilder.ForceRebuildLayoutImmediate(panel.parent.transform as RectTransform);
        }
    }
}