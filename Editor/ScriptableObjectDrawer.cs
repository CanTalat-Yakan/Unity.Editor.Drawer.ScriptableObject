#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

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

            bool hasVisibleProperties = false;
            if (_editor == null || _editor.target != property.objectReferenceValue)
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);

            if (_editor != null)
            {
                SerializedObject so = _editor.serializedObject;
                SerializedProperty iterator = so.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    if (iterator.name == "m_Script")
                        continue;
                    hasVisibleProperties = true;
                    break;
                }
            }

            if (!hasVisibleProperties)
            {
                property.isExpanded = false;
                return;
            }

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none, true);
            if (!property.isExpanded)
                return;

            EditorGUI.indentLevel++;
            if (_editor != null)
            {
                SerializedObject so = _editor.serializedObject;
                so.Update();
                EditorGUI.BeginChangeCheck();

                SerializedProperty iterator = so.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    if (iterator.name == "m_Script") // Skip script reference
                        continue;

                    EditorGUILayout.PropertyField(iterator, true); // Draw children (lists, arrays, etc.)
                    enterChildren = false;
                }

                if (EditorGUI.EndChangeCheck())
                    so.ApplyModifiedProperties();
            }
            EditorGUI.indentLevel--;

            GUILayout.Space(10);
        }
    }
}
#endif