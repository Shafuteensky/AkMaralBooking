using UnityEngine;
using Extensions.Coroutines;
#if DOTWEEN
using DG.Tweening;
#endif

namespace Extensions.UIWindows
{
    public class ButtonOpenUIWindowAnimated : ButtonOpenUIWindow
    {
#if DOTWEEN
        [Header("Анимация"), Space]
        [SerializeField] protected DOTweenAnimation beforeOpenAnimation;
#endif

        public override void OnButtonClick()
        {
#if DOTWEEN
            if (openMode == UIWindowOpenMode.Pop && beforeOpenAnimation)
            {
                beforeOpenAnimation.DORestart();
                CoroutineDelay.Run(this, beforeOpenAnimation.duration, base.OnButtonClick);
                return;
            }
#endif
            base.OnButtonClick();
        }
    }
}