using Extensions.Data;
using UnityEngine;

namespace StarletBooking.Data
{
    public class AndroidBackupNotifier : MonoBehaviour
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaObject _backupManager;
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var go = new GameObject("[AndroidBackupNotifier]");
            DontDestroyOnLoad(go);
            go.AddComponent<AndroidBackupNotifier>();
#endif
        }

        private void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var context = player.GetStatic<AndroidJavaObject>("currentActivity");
            _backupManager = new AndroidJavaObject("android.app.backup.BackupManager", context);

            JsonSaveLoad.onAfterSave += OnAfterSave;
#endif
        }

        private void OnDestroy()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            JsonSaveLoad.onAfterSave -= OnAfterSave;
            _backupManager?.Dispose();
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private void OnAfterSave(string key)
        {
            _backupManager?.Call("dataChanged");
        }
#endif
    }
}
