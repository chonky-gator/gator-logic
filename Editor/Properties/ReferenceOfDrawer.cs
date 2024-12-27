using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GatOR.Logic.Properties;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace GatOR.Logic.Editor.Editor.Properties
{
	[CustomPropertyDrawer(typeof(ReferenceOf<>))]
	public class ReferenceOfDrawer : PropertyDrawer
	{
		// This code ended up being a mess of hacks, due to unity's serialization and IMGUI calls
		// And specially to make it compatible with the [CreateAssetButton] functionality
		
		private static readonly Dictionary<Type, TypeLookup> InfoForTypes = new();
		private static readonly GUIContent TypeLabel = new("Type");
		
		// Note: This cache is never properly cleaned, is a hackish solution to remember
		// what concrete UnityEngine.Object we selected
		private static readonly Dictionary<string, Type> SelectedConcreteTypes = new();

		private static readonly float LineHeight = EditorGUIUtility.singleLineHeight + 2f;

		#region Inspector Draw
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return GetPropertyHeightStatic(property, label, fieldInfo);
		}
		
		public static float GetPropertyHeightStatic(SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
		{
			using var props = new ReferenceOfProps(property, fieldInfo);
			return props.GetHeight(property.propertyPath);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			OnGUIStatic(position, property, label, fieldInfo);
		}
		
		public static void OnGUIStatic(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
		{
			using var props = new ReferenceOfProps(property, fieldInfo);
			var propertyPath = property.propertyPath;
			var expectedType = props.GetExpectedType();
			var typesLookup = GetOrCreateCacheForType(expectedType);
			
			var drawingAt = position;
			drawingAt.height = EditorGUIUtility.singleLineHeight;
			
			label.text += $" <color=#888888><{expectedType.FullName}></color>";
			EditorGUI.LabelField(drawingAt, label, GUIStyles.RichTextLabelStyle);

			EditorGUI.indentLevel++;
			drawingAt.y += EditorGUIUtility.singleLineHeight;

			var previousType = props.GetCurrentType(propertyPath);
			var previousTypeIndex = typesLookup.GetIndexForType(previousType);
			int newTypeIndex = EditorGUI.IntPopup(drawingAt, TypeLabel, previousTypeIndex,
				typesLookup.TypeNames, null);
			var newType = typesLookup.GetType(newTypeIndex);

			if (newTypeIndex != previousTypeIndex)
				props.SelectNewType(newType, propertyPath);

			drawingAt.y += LineHeight;
			props.Draw(drawingAt, newType, GetReferenceKind(newType));

			EditorGUI.indentLevel--;
		}
		#endregion

		private static ReferenceKind GetReferenceKind([CanBeNull] Type type)
		{
			if (type == null)
				return ReferenceKind.Null;
				
			return typeof(UnityEngine.Object).IsAssignableFrom(type) ? ReferenceKind.UnityObject
				: ReferenceKind.SerializedReference;
		}

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
					if (uniqueNameChecker.Add(name.text))
						continue;
					
					Debug.LogAssertion($"Name \"{name.text}\" already exists.");
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
			private readonly SerializedProperty serializedReferenceProp;
			private readonly SerializedProperty unityObjectProp;
			private readonly FieldInfo fieldInfo;

			public ReferenceOfProps(SerializedProperty from, FieldInfo fieldInfo)
			{
				serializedReferenceProp = from.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference));
				unityObjectProp = from.FindPropertyRelative(nameof(ReferenceOf<object>.unityObject));
				this.fieldInfo = fieldInfo;
			}

			public bool HasCreateAssetButtonAttribute()
			{
				return fieldInfo.GetCustomAttribute<CreateAssetButtonAttribute>() != null;
			}

			public void SelectNewType(Type type, string propertyPath)
			{
				switch (GetReferenceKind(type))
				{
					case ReferenceKind.Null:
						serializedReferenceProp.managedReferenceValue = null;
						unityObjectProp.objectReferenceValue = null;
						SelectedConcreteTypes.Remove(propertyPath);
						break;
					case ReferenceKind.SerializedReference:
						unityObjectProp.objectReferenceValue = null;
						serializedReferenceProp.managedReferenceValue = Activator.CreateInstance(type);
						SelectedConcreteTypes.Remove(propertyPath);
						break;
					case ReferenceKind.UnityObject:
						serializedReferenceProp.managedReferenceValue = null;
						SelectedConcreteTypes[propertyPath] = type;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public float GetHeight(string propertyPath)
			{
				var height = LineHeight * 2f;
				var currentType = GetCurrentType(propertyPath);
				height += GetReferenceKind(currentType) switch
				{
					ReferenceKind.Null => 0f,
					ReferenceKind.SerializedReference => EditorGUI.GetPropertyHeight(serializedReferenceProp, true),
					ReferenceKind.UnityObject => EditorGUI.GetPropertyHeight(unityObjectProp, true),
					_ => throw new ArgumentOutOfRangeException()
				};
				return height;
			}

			public void Draw(Rect position, Type type, ReferenceKind referenceKind)
			{
				switch (referenceKind)
				{
					case ReferenceKind.SerializedReference:
						EditorGUI.PropertyField(position, serializedReferenceProp, true);
						break;
					case ReferenceKind.UnityObject:
						if (typeof(ScriptableObject).IsAssignableFrom(type) && HasCreateAssetButtonAttribute())
						{
							CreateAssetButtonDrawer.OnGUIStatic(position, unityObjectProp, null, type);
						}
						else
						{
							unityObjectProp.objectReferenceValue = EditorGUI.ObjectField(position,
								unityObjectProp.objectReferenceValue, type,
								unityObjectProp.serializedObject.targetObject);
						}
						break;
					case ReferenceKind.Null:
						// Don't draw anything extra here
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(referenceKind), referenceKind, null);
				}
			}

			/// <summary>What type is the <see cref="ReferenceOf{TReference}"/> expecting.</summary>
			public Type GetExpectedType() =>
				EditorUtils.GetTypeWithFullName(serializedReferenceProp.managedReferenceFieldTypename);

			/// <summary>Gets the type of the currently assign object.</summary>
			/// <returns>The type of the object, can be null.</returns>
			[CanBeNull]
			public Type GetCurrentType(string propertyPath)
			{
				var currentType = SelectedConcreteTypes.GetValueOrDefault(propertyPath);
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
			}
		}
	}
}