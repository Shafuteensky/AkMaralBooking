using UnityEngine;
using Extensions.Coroutines;
#if DOTWEEN
using DG.Tweening;
#endif

namespace Extensions.UIWindows
{
    public class ButtonCloseUIWindowAnimated : ButtonCloseUIWindow
    {
#if DOTWEEN
        [Header("Анимация"), Space]
        [SerializeField] protected DOTweenAnimation beforeCloseAnimation;
#endif

        public override void OnButtonClick()
        {
#if DOTWEEN
            if (beforeCloseAnimation)
            {
                beforeCloseAnimation.DORestart();
                CoroutineDelay.Run(this, beforeCloseAnimation.duration, base.OnButtonClick);
                return;
            }
#endif
            base.OnButtonClick();
        }
    }
}