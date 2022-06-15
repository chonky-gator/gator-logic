using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        private const string KvpsFieldName = nameof(SerializableDictionary<string, string>.kvps);
        private const string ToAddFieldName = nameof(SerializableDictionary<string, string>.add);
        private const string ConflictFieldName = nameof(SerializableDictionary<string, string>.conflict);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var kvps = property.FindPropertyRelative(KvpsFieldName);
            kvps.isExpanded = true;

            var height = EditorGUI.GetPropertyHeight(property, label, true);

            var conflict = property.FindPropertyRelative(ConflictFieldName);
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

            var toAdd = property.FindPropertyRelative(ToAddFieldName);
            propPosition.y += propPosition.height;
            
            var kvps = property.FindPropertyRelative(KvpsFieldName);

            var addPosition = propPosition;
            EditorGUI.PropertyField(addPosition, toAdd);
            addPosition.width = 100f;
            if (GUI.Button(addPosition, "Add with key:"))
            {
                var newIndex = kvps.arraySize;
                kvps.InsertArrayElementAtIndex(newIndex);

                var newItem = kvps.GetArrayElementAtIndex(newIndex);
                var newItemKey = newItem.FindPropertyRelative("key");
                newItemKey.CopyValueFrom(toAdd);
                
                toAdd.ClearValue();
            }

            propPosition.y += propPosition.height;
            EditorGUI.PropertyField(propPosition, kvps);

            var conflict = property.FindPropertyRelative(ConflictFieldName);
            if (conflict.boolValue)
            {
                propPosition.y += EditorGUI.GetPropertyHeight(kvps, label, true);
                EditorGUI.HelpBox(propPosition, "Key/s are conflicting or is/are null", MessageType.Error);
            }

            EditorGUI.indentLevel--;
        }
    }
}
