#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Unity.Essentials
{
    /// <summary>
    /// Provides a custom property drawer for fields of type <see cref="ScriptableObject"/> in the Unity Editor.
    /// </summary>
    /// <remarks>This drawer allows for the inline editing of <see cref="ScriptableObject"/> instances in the
    /// Inspector. When the property is expanded, the drawer displays the Inspector GUI for the referenced <see
    /// cref="ScriptableObject"/>.</remarks>
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ScriptableObjectDrawer : PropertyDrawer
    {
        private Editor _editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyPath.Contains("Array.data"))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            EditorGUI.PropertyField(position, property, label, true);

            if (property.objectReferenceValue == null)
            {
                property.isExpanded = false;
                return;
            }

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none, true);
            if (!property.isExpanded)
                return;

            EditorGUI.indentLevel++;
            {
                if (_editor == null || _editor.target != property.objectReferenceValue)
                    Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);

                _editor?.OnInspectorGUI();
            }
            EditorGUI.indentLevel--;
        }
    }
}
#endif