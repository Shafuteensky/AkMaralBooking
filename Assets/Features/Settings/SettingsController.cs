using UnityEngine;

namespace StarletBooking.Settings
{
    /// <summary>
    /// Контроллер настроек
    /// </summary>
    public sealed class SettingsController : MonoBehaviour
    {
        [Range(0, 240)]
        [SerializeField] private int targetFrameRate = 60;
        
        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }
    }
}