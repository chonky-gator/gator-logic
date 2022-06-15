using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        private const string KvpsFieldName = nameof(SerializableDictionary<string, string>.kvps);
        // private const string ToAddFieldName = nameof(SerializableDictionary<string, string>.add);
        private const string ConflictFieldName = nameof(SerializableDictionary<string, string>.conflict);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var kvps = property.FindPropertyRelative(KvpsFieldName);
            var height = EditorGUI.GetPropertyHeight(kvps, label, true);

            var conflict = property.FindPropertyRelative(ConflictFieldName);
            if (conflict.boolValue)
                height += EditorGUIUtility.singleLineHeight;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propPosition = position;
            propPosition.height = EditorGUIUtility.singleLineHeight;

            var kvps = property.FindPropertyRelative(KvpsFieldName);
            EditorGUI.PropertyField(propPosition, kvps);

            var conflict = property.FindPropertyRelative(ConflictFieldName);
            if (!conflict.boolValue)
                return;
            
            propPosition.y += EditorGUI.GetPropertyHeight(kvps, label, true);
            EditorGUI.HelpBox(propPosition, "Key/s are conflicting or is/are null", MessageType.Error);
        }
    }
}
