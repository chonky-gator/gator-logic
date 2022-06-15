using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor.Editor.Properties
{
	[CustomPropertyDrawer(typeof(SerializableHashSet<>))]
	public class SerializableHashSetDrawer : PropertyDrawer
	{
		private const string SerializedArrayPath = nameof(SerializableHashSet<string>.serializedArray);
		private const string ConflictPath = nameof(SerializableHashSet<string>.conflict);

		public override void OnGUI(Rect position, SerializedProperty property,
			GUIContent label)
		{
			var propPosition = position;
			propPosition.height -= EditorGUIUtility.singleLineHeight;

			var serializedArray = property.FindPropertyRelative(SerializedArrayPath);
			EditorGUI.PropertyField(propPosition, serializedArray, label);
			
			propPosition.y += propPosition.height;
			propPosition.height = EditorGUIUtility.singleLineHeight;
			var conflict = property.FindPropertyRelative(ConflictPath);
			if (conflict.boolValue)
				EditorGUI.HelpBox(propPosition, "List contains repeated values", MessageType.Error);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var serializedArray = property.FindPropertyRelative(SerializedArrayPath);
			var height = EditorGUI.GetPropertyHeight(serializedArray, label);
			
			var conflict = property.FindPropertyRelative(ConflictPath);
			if (conflict.boolValue)
				height += EditorGUIUtility.singleLineHeight;

			return height;
		}
	}
}