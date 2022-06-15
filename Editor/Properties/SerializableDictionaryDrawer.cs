using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GatOR.Logic.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var kvps = property.FindPropertyRelative(SerializableDictionaryNames.Kvps);
            kvps.isExpanded = true;

            var height = EditorGUI.GetPropertyHeight(property, label, true);

            var conflict = property.FindPropertyRelative(SerializableDictionaryNames.Conflict);
            if (conflict.boolValue)
                height += EditorGUIUtility.singleLineHeight;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propPosition = position;
            propPosition.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(propPosition, property.isExpanded, label, true);
            if (!property.isExpanded)
                return;

            EditorGUI.indentLevel++;

            var toAdd = property.FindPropertyRelative(SerializableDictionaryNames.ToAdd);
            propPosition.y += propPosition.height;

            var addPosition = propPosition;
            EditorGUI.PropertyField(addPosition, toAdd);
            addPosition.width = 100f;
            GUI.Button(addPosition, "Add with key:");

            propPosition.y += propPosition.height;
            var kvps = property.FindPropertyRelative(SerializableDictionaryNames.Kvps);
            EditorGUI.PropertyField(propPosition, kvps);

            var conflict = property.FindPropertyRelative(SerializableDictionaryNames.Conflict);
            if (conflict.boolValue)
            {
                propPosition.y += EditorGUI.GetPropertyHeight(kvps, label, true);
                EditorGUI.HelpBox(propPosition, "Key/s are conflicting or is/are null", MessageType.Error);
            }

            EditorGUI.indentLevel--;
        }
    }
}
