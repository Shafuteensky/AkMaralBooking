using DG.Tweening;
using Extensions.Generics;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Data_UI_view
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
        
        private bool isCollapsed;
        
        public override void OnButtonClickAction()
        {
            isCollapsed = !isCollapsed;
            
            if (fitter != null) fitter.enabled = !isCollapsed;
            
            if (isCollapsed && panel != null)
            {
                panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, collapsedHeight);
                if (collapseAnimation != null) collapseAnimation.tween.Restart();
            }
            else
                if (collapseAnimation != null) collapseAnimation.tween.PlayBackwards();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(panel.parent.transform as RectTransform);
        }
    }
}