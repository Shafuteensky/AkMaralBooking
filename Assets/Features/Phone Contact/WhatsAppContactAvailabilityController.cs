namespace StarletBooking.PhoneContact
{
    /// <summary>
    /// Делает кнопку WhatsApp доступной если у выбранного клиента есть номер телефона
    /// и WhatsApp установлен на устройстве.
    /// </summary>
    public sealed class WhatsAppContactAvailabilityController : PhoneContactAvailabilityBase
    {
        private bool isWhatsAppInstalled;

        protected override void Awake()
        {
            base.Awake();
            isWhatsAppInstalled = CheckWhatsAppInstalled();
        }

        protected override bool ResolveAvailability(bool hasNumber) => hasNumber && isWhatsAppInstalled;

        private static bool CheckWhatsAppInstalled()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (var unityPlayer = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var activity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity"))
                using (var pm = activity.Call<UnityEngine.AndroidJavaObject>("getPackageManager"))
                {
                    pm.Call<UnityEngine.AndroidJavaObject>("getPackageInfo", "com.whatsapp", 0);
                    return true;
                }
            }
            catch (UnityEngine.AndroidJavaException)
            {
                return false;
            }
#else
            return false;
#endif
        }
    }
}
