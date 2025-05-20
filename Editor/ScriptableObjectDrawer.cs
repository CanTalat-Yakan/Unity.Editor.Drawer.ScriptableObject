#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Unity.Essentials
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ScriptableObjectDrawer : PropertyDrawer
    {
        private Editor editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the main object field
            EditorGUI.PropertyField(position, property, label, true);

            if (property.objectReferenceValue == null)
            {
                // If null, ensure foldout is collapsed and return early
                property.isExpanded = false;
                return;
            }

            // Draw foldout only if object reference exists
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                // Create editor only if needed
                if (editor == null || editor.target != property.objectReferenceValue)
                    Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

                // Double check editor is valid before drawing
                if (editor != null)
                    editor.OnInspectorGUI();


                EditorGUI.indentLevel--;
            }
        }
    }
}
#endif