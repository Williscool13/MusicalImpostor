#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScriptableReference<>), true)]
public class ScriptableReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
        SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
        SerializedProperty variable = property.FindPropertyRelative("Variable");

        Rect buttonRect = new Rect(position);
        buttonRect.width = 75; // Adjust the width as needed
        position.xMin = buttonRect.xMax;

        int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, new[] { "Use Constant", "Use Variable" });

        useConstant.boolValue = result == 0;

        if (useConstant.boolValue) {
            EditorGUI.PropertyField(position, constantValue, GUIContent.none);
        }
        else {
            EditorGUI.PropertyField(position, variable, GUIContent.none);
        }

        if (EditorGUI.EndChangeCheck()) {
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }
}
#endif