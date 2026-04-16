#if UNITY_EDITOR
using Extensions.Helpers;
using UnityEditor;
using UnityEngine;

namespace Extensions.UIWindows.Editor
{
    [CustomEditor(typeof(ButtonOpenUIWindow), true)]
    public class ButtonOpenUIWindowEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target.GetType() != typeof(ButtonOpenUIWindow))
                return;

            GUILayout.Space(8f);

            if (GUILayout.Button("Convert To ButtonOpenUIWindowAnimated"))
            {
                Converter.ConvertComponent<ButtonOpenUIWindow, ButtonOpenUIWindowAnimated>((ButtonOpenUIWindow)target);
            }
        }
    }
}
#endif