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
        
        [SerializeField] private DOTweenAnimation animation;
        
        private bool isCollapsed;
        
        public override void OnButtonClickAction()
        {
            isCollapsed = !isCollapsed;
            
            if (fitter != null) fitter.enabled = !isCollapsed;
            
            if (isCollapsed && panel != null)
            {
                panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, collapsedHeight);
                if (animation != null) animation.tween.Restart();
            }
            else
                if (animation != null) animation.tween.PlayBackwards();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(panel.parent.transform as RectTransform);
        }
    }
}