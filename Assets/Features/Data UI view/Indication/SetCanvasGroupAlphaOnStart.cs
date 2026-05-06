using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Выставление альфы группы в 0 
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class SetCanvasGroupAlphaOnStart : MonoBehaviour
    {
        private void Start() => GetComponent<CanvasGroup>().alpha = 0f;
    }
}