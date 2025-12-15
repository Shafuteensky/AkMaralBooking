using UnityEngine;
using UnityEngine.UI;

namespace Extentions
{
    /// <summary>
    /// Абстракция кнопки
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AbstractOnClickButton : MonoBehaviour
    {
        protected Button button = default;

        protected virtual void Awake() => button = GetComponent<Button>();

        protected virtual void OnEnable() => button.onClick.AddListener(OnButtonClick);

        protected virtual void OnDisable() => button.onClick.RemoveListener(OnButtonClick);

        protected abstract void OnButtonClick();
    }
}
