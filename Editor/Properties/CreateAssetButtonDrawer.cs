using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor.Editor.Properties
{
	[CustomPropertyDrawer(typeof(CreateAssetButtonAttribute))]
	public class CreateAssetButtonDrawer : PropertyDrawer
	{
		private static readonly Regex SelectedTypeRegex = new(@"PPtr<\$(.*)>");
		
		public override void OnGUI(Rect position, SerializedProperty property,
			GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference)
			{
				EditorGUI.LabelField(position, label.text,
					$"[{nameof(CreateAssetButtonAttribute)}] can only be used on unity object fields");
				return;
			}

			var itemPosition = position;
			itemPosition.width = position.width * 0.75f;
			EditorGUI.PropertyField(itemPosition, property, label);

			const float padding = 4f;
			itemPosition.x += itemPosition.width + padding;
			itemPosition.width = position.width * 0.25f - padding;
			if (!GUI.Button(itemPosition, "Create"))
				return;
			
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

		private void CreateObject()
		{
			
		}
	}
}