#if UNITY_EDITOR
using Extensions.Helpers;
using UnityEditor;
using UnityEngine;

namespace Extensions.UIWindows.Editor
{
    [CustomEditor(typeof(ButtonActionOpenUIWindow), true)]
    public class ButtonOpenUIWindowEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target.GetType() != typeof(ButtonActionOpenUIWindow))
                return;

            GUILayout.Space(8f);

            if (GUILayout.Button("Convert To ButtonOpenUIWindowAnimated"))
            {
                Converter.ConvertComponent<ButtonActionOpenUIWindow, ButtonActionOpenUIWindowAnimated>((ButtonActionOpenUIWindow)target);
            }
        }
    }
}
#endif