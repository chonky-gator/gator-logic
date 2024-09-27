using System.Reflection;
using System.Text.RegularExpressions;
using GatOR.Logic.Properties;
using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor.Editor.Properties
{
	[CustomPropertyDrawer(typeof(CreateAssetButtonAttribute))]
	public class CreateAssetButtonDrawer : PropertyDrawer
	{
		private static readonly Regex SelectedTypeRegex = new(@"PPtr<\$(.*)>");

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return IsReferenceOf(fieldInfo) ?
				ReferenceOfDrawer.GetPropertyHeightStatic(property, label, fieldInfo) :
				base.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property,
			GUIContent label)
		{
			if (IsReferenceOf(fieldInfo))
				ReferenceOfDrawer.OnGUIStatic(position, property, label, fieldInfo);
			else
				OnGUIStatic(position, property, label);
		}

		public static bool IsReferenceOf(FieldInfo fieldInfo)
		{
			var fieldType = EditorUtils.GetIndividualType(fieldInfo);
			return fieldType.GetGenericTypeDefinition() == typeof(ReferenceOf<>);
		}
		
		public static void OnGUIStatic(Rect position, SerializedProperty property,
			GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference)
			{
				EditorGUI.LabelField(position, label?.text,
					$"[{nameof(CreateAssetButtonAttribute)}] can only be used on unity object fields");
				return;
			}

			var itemPosition = position;
			itemPosition.width = position.width * 0.75f;
			EditorGUI.PropertyField(itemPosition, property, label);

			const float padding = 4f;
			itemPosition.x += itemPosition.width + padding;
			itemPosition.width = position.width * 0.25f - padding;
			if (GUI.Button(itemPosition, "Create"))
				CreateObject(property);
		}

		private static void CreateObject(SerializedProperty property)
		{
			var type = SelectedTypeRegex.Match(property.type).Groups[1].Value;
			var path = EditorUtility.SaveFilePanelInProject("Save Asset", type,
				"asset", "Save asset with name");

			if (string.IsNullOrEmpty(path))
			{
				GUIUtility.ExitGUI(); // Prevents InvalidOperationException
				return;
			}

			var asset = ScriptableObject.CreateInstance(type);
			property.objectReferenceValue = asset;
			property.serializedObject.ApplyModifiedProperties();
			
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();

			EditorGUIUtility.PingObject(asset);
			GUIUtility.ExitGUI(); // Prevents InvalidOperationException
		}
	}
}