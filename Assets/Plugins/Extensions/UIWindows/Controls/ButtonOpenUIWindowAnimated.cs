using UnityEngine;
using Extensions.Coroutines;
using UnityEditor;
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

#if UNITY_EDITOR
        [ContextMenu("Convert to simple (unanimated)")]
        private void ConvertToSimple()
        {
            GameObject go = gameObject;

            Undo.RecordObject(go, "Convert to simple (unanimated)");

            UIWindowID cachedWindowToOpen = UIWindowToOpen;
            bool cachedNeedToCloseThis = needToCloseThis;
            UIWindowOpenMode cachedOpenMode = openMode;

            DestroyImmediate(this, true);

            ButtonOpenUIWindow simpleButton = go.AddComponent<ButtonOpenUIWindow>();
            simpleButton.UpdateParams(cachedWindowToOpen, cachedNeedToCloseThis, cachedOpenMode);

            EditorUtility.SetDirty(go);
        }
#endif
    }
}