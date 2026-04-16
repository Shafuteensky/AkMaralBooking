#if UNITY_EDITOR
using Extensions.Helpers;
using UnityEditor;
using UnityEngine;

namespace Extensions.UIWindows.Editor
{
    [CustomEditor(typeof(ButtonCloseUIWindowAnimated))]
    public class ButtonCloseUIWindowAnimatedEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(8f);

            if (GUILayout.Button("Convert To ButtonCloseUIWindow"))
            {
                Converter.ConvertComponent<ButtonCloseUIWindowAnimated, ButtonCloseUIWindow>((ButtonCloseUIWindowAnimated)target);
            }
        }
    }
}
#endif