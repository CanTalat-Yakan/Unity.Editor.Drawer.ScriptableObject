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

        /// <summary>
        /// Renders the custom GUI for a serialized property in the Unity Editor.
        /// </summary>
        /// <remarks>This method customizes the rendering of a serialized property in the Unity Editor. 
        /// If the property has no assigned object reference, it displays a standard property field and collapses any
        /// expanded state. If the property has an object reference, it displays a foldout to expand or collapse
        /// additional details. When expanded, the method renders the inspector GUI for the referenced object using a
        /// cached editor instance.</remarks>
        /// <param name="position">The rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The serialized property to render.</param>
        /// <param name="label">The label to display alongside the property field.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            if (property.objectReferenceValue == null)
            {
                property.isExpanded = false;
                return;
            }

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
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