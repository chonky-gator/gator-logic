using System;
using System.Collections.Generic;
using System.Linq;
using GatOR.Logic.Properties;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor.Editor.Properties
{
	[CustomPropertyDrawer(typeof(ReferenceOf<>))]
	public class ReferenceOfDrawer : PropertyDrawer
	{
		private static readonly Dictionary<Type, TypeLookup> InfoForTypes = new();

		#region Inspector Draw
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 2;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using var props = new ReferenceOfProps(property);
			var typesLookup = GetOrCreateCacheForType(props.ExpectedType);
			
			var drawingAt = position;
			drawingAt.height = EditorGUIUtility.singleLineHeight;
			
			label.text += $" <color=#888888><{props.ExpectedType.FullName}> type</color>";
			EditorGUI.LabelField(drawingAt, label, GUIStyles.RichTextLabelStyle);

			drawingAt.y += EditorGUIUtility.singleLineHeight;

			EditorGUI.indentLevel++;

			var previousType = props.GetCurrentType();
			var previousTypeIndex = typesLookup.GetIndexForType(previousType);
			int newTypeIndex = EditorGUI.IntPopup(drawingAt, GUIContent.none, previousTypeIndex,
				typesLookup.TypeNames, null);

			if (newTypeIndex != previousTypeIndex)
			{
				var newType = typesLookup.GetType(newTypeIndex);
				var newTypeName = newType != null ? EditorUtils.AsFullnameType(newType) : null;
				props.SelectedType = newTypeName;
				Debug.Log($"[{newTypeIndex}]: {newTypeName}");
			}

			EditorGUI.indentLevel--;
		}
		#endregion

		private static TypeLookup GetOrCreateCacheForType(Type type)
		{
			if (InfoForTypes.TryGetValue(type, out var cache))
				return cache;

			var newLookup = new TypeLookup(type);
			InfoForTypes[type] = newLookup;
			return newLookup;
		}

		private readonly struct TypeLookup
		{
			private static readonly GUIContent[] BaseNamesList = { new("<Null>") };
			
			public readonly GUIContent[] TypeNames;
			private readonly Type[] inheritingTypes;

			public TypeLookup(Type type)
			{
				inheritingTypes = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(a => a.GetTypes())
					.Where(t => !t.IsAbstract && type.IsAssignableFrom(t))
					.ToArray();
				GUIContent[] typeNames = BaseNamesList
					.Concat(inheritingTypes.Select(t => new GUIContent(t.FullName)))
					.ToArray();
				var uniqueNameChecker = new HashSet<string>();
				foreach (GUIContent name in typeNames)
				{
					if (uniqueNameChecker.Contains(name.text))
					{
						Debug.LogAssertion($"Name \"{name.text}\" already exists.");
						continue;
					}
					uniqueNameChecker.Add(name.text);
				}

				TypeNames = typeNames;
			}

			[CanBeNull]
			public Type GetType(int index)
			{
				return index != 0 ? inheritingTypes[index - 1] : null;
			}

			public int GetIndexForType(Type type)
			{
				if (type == null)
					return 0;

				var index = Array.IndexOf(inheritingTypes, type);
				return index >= 0 ? (index + 1) : -1;
			}
		}

		private readonly struct ReferenceOfProps : IDisposable
		{
			/// <summary>What type is the <see cref="ReferenceOf{TReference}"/> expecting.</summary>
			public readonly Type ExpectedType;
			
			private readonly SerializedProperty serializedReferenceProp;
			private readonly SerializedProperty unityObjectProp;
			private readonly SerializedProperty selectedTypeProp;
			public string SelectedType
			{
				get => selectedTypeProp.stringValue;
				set => selectedTypeProp.stringValue = value;
			}

			public ReferenceOfProps(SerializedProperty from)
			{
				serializedReferenceProp = from.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference));
				ExpectedType = EditorUtils.GetTypeWithFullName(serializedReferenceProp.managedReferenceFieldTypename);
				unityObjectProp = from.FindPropertyRelative(nameof(ReferenceOf<object>.unityObject));
				selectedTypeProp = from.FindPropertyRelative(nameof(ReferenceOf<object>.selectedConcreteType));
			}

			/// <summary>Gets the type of the currently assign object.</summary>
			/// <returns>The type of the object, can be null.</returns>
			[CanBeNull]
			public Type GetCurrentType()
			{
				var currentType = EditorUtils.GetTypeWithFullName(SelectedType);
				if (currentType != null)
					return currentType;

				var unityObject = unityObjectProp.objectReferenceValue;
				if (unityObject != null)
					return unityObject.GetType();

				var serializedType =
					EditorUtils.GetTypeWithFullName(serializedReferenceProp.managedReferenceFullTypename);
				return serializedType;
			}
			
			public void Dispose()
			{
				serializedReferenceProp.Dispose();
				unityObjectProp.Dispose();
				selectedTypeProp.Dispose();
			}
		}
	}
}