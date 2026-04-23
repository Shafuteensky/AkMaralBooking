using UnityEditor;
using UnityEngine;

namespace EZCalendarWeeklyView
{
    [CustomEditor(typeof(WeekViewController))]
    public class WeekViewControllerEditor : Editor
    {
        SerializedProperty useScreenWidthForSwipeProp;
        SerializedProperty swipeThresholdProp;

        private void OnEnable()
        {
            // Link the properties
            useScreenWidthForSwipeProp = serializedObject.FindProperty("useScreenWidthForSwipe");
            swipeThresholdProp = serializedObject.FindProperty("swipeThreshold");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            serializedObject.Update();

            // Draw default inspector excluding specific fields
            DrawPropertiesExcluding(serializedObject, "useScreenWidthForSwipe", "swipeThreshold");

            // Add a space in the inspector
            EditorGUILayout.Space();

            // Draw the toggle for useScreenWidthForSwipe
            EditorGUILayout.PropertyField(useScreenWidthForSwipeProp);

            // Only show swipeThreshold if useScreenWidthForSwipe is false
            if (!useScreenWidthForSwipeProp.boolValue)
            {
                swipeThresholdProp.floatValue = EditorGUILayout.Slider("Swipe Threshold", swipeThresholdProp.floatValue, 1f, 2000f);
            }

            // Apply any changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}