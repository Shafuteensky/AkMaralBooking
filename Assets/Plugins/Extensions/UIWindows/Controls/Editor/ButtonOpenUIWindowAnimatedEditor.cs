#if UNITY_EDITOR
using Extensions.Helpers;
using UnityEditor;
using UnityEngine;

namespace Extensions.UIWindows.Editor
{
    [CustomEditor(typeof(ButtonOpenUIWindowAnimated))]
    public class ButtonOpenUIWindowAnimatedEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ButtonOpenUIWindowAnimated button = (ButtonOpenUIWindowAnimated)target;

            if (button.OpenMode != UIWindowOpenMode.Pop)
            {
                EditorGUILayout.HelpBox(
                    "Анимация открытия используется только в режиме Pop. В режиме Forward она не применяется.",
                    MessageType.Warning
                );
            }

            GUILayout.Space(8f);

            if (GUILayout.Button("Convert To ButtonOpenUIWindow"))
            {
                Converter.ConvertComponent<ButtonOpenUIWindowAnimated, ButtonOpenUIWindow>((ButtonOpenUIWindowAnimated)target);
            }
        }
    }
}
#endif