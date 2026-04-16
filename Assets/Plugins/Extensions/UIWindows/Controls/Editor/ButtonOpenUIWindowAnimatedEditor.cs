#if UNITY_EDITOR
using UnityEditor;

namespace Extensions.UIWindows.Editor
{
    [CustomEditor(typeof(ButtonOpenUIWindowAnimated))]
    public class ButtonOpenUIWindowAnimatedEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ButtonOpenUIWindowAnimated button = (ButtonOpenUIWindowAnimated)target;

            if (button == null) return;

            if (button.OpenMode != UIWindowOpenMode.Pop)
            {
                EditorGUILayout.HelpBox(
                    "Анимация открытия используется только в режиме Pop. В режиме Forward она не применяется.",
                    MessageType.Warning
                );
            }
        }
    }
}
#endif