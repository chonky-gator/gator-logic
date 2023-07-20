/*
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using GatOR.Logic.Properties;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(ReferenceOf<>))]
    public class ReferenceOfDrawerOld : PropertyDrawer
    {
        private struct Cache
        {
            public Type[] inheritingTypes;
            public GUIContent[] typeNames;
        }

        private static readonly Dictionary<Type, Cache> InfoForTypes = new Dictionary<Type, Cache>();

        private static readonly List<ICustomReferenceOf> CustomOptions = new List<ICustomReferenceOf>()
        {
            new NullCustomReferenceOf(),
            new UnityObjectCustomReferenceOf(),
        };

        private static float _baseHeight;

        private int _selectedPropertyDrawerIndex = -1;
        private ICustomReferenceOf SelectedPropertyDrawer => (_selectedPropertyDrawerIndex >= 0)
            ? CustomOptions[_selectedPropertyDrawerIndex] : null;

        private static Cache GetOrCreateInfoForType(Type type)
        {
            if (InfoForTypes.TryGetValue(type, out Cache cache))
            {
                // Debug.Log($"Got cache for type: {type}");
                return cache;
            }

            Type[] inheritingTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && type.IsAssignableFrom(t))
                .ToArray();
            GUIContent[] typeNames = CustomOptions
                .Select(option => option.Name)
                .Concat(inheritingTypes.Select(t => new GUIContent(t.Name)))
                .ToArray();
            var uniqueNameChecker = new HashSet<string>();
            foreach (GUIContent name in typeNames)
            {
                if (uniqueNameChecker.Contains(name.text))
                    throw new Exception($"Name \"{name.text}\" already exists.");

                uniqueNameChecker.Add(name.text);
            }

            var newCache = new Cache
            {
                inheritingTypes = inheritingTypes,
                typeNames = typeNames,
            };
            InfoForTypes[type] = newCache;
            return newCache;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var referenceProperty = property.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference));
            var referenceType = EditorUtils.GetTypeWithFullName(referenceProperty.managedReferenceFieldTypename);
            _selectedPropertyDrawerIndex = CustomOptions.FindIndex(x => x.IsSelected(property, referenceType));
            
            _baseHeight = base.GetPropertyHeight(property, label);
            var referenceHeight = SelectedPropertyDrawer?.GetHeight(property) ?? EditorGUI.GetPropertyHeight(referenceProperty, label);
            return _baseHeight + referenceHeight + 6f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var typeReference = property.FindPropertyRelative(nameof(ReferenceOf<object>.type));
            using var referenceProperty = property.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference));
            using var selectedType = property.FindPropertyRelative(nameof(ReferenceOf<object>.selectedType));
            using var unityObjectReference = property.FindPropertyRelative(nameof(ReferenceOf<object>.unityObject));

            Type referenceType = EditorUtils.GetTypeWithFullName(referenceProperty.managedReferenceFieldTypename);

            var info = GetOrCreateInfoForType(referenceType);
            Type[] inheritingTypes = info.inheritingTypes;
            GUIContent[] typeNames = info.typeNames;

            int previousSelectedTypeIndex = ((ReferenceOfType)typeReference.enumValueIndex == ReferenceOfType.Null) ?
                0 : selectedType.intValue;
            label.text += $" <color=#888888><{referenceType.Name}> type</color>";

            position.height = _baseHeight;
            position.y += 2f;
            Rect labelPosition = position;
            labelPosition.width *= 0.5f;
            EditorGUI.LabelField(labelPosition, label, GUIStyles.RichTextLabelStyle);

            labelPosition.x += labelPosition.width;
            int currentSelectedTypeIndex = EditorGUI.IntPopup(labelPosition, null, previousSelectedTypeIndex, typeNames, null);
            if (currentSelectedTypeIndex != previousSelectedTypeIndex)
                SetType(currentSelectedTypeIndex);

            EditorGUI.indentLevel++;
            position.y += position.height + 2f;
            
            var selectedPropertyDrawer = currentSelectedTypeIndex < CustomOptions.Count ? CustomOptions[currentSelectedTypeIndex] : null;
            if (selectedPropertyDrawer == null)
            {
                switch ((ReferenceOfType)typeReference.enumValueIndex)
                {
                    case ReferenceOfType.SerializedReference:
                        EditorGUI.PropertyField(position, referenceProperty, true);
                        break;
                    case ReferenceOfType.UnityObject:
                        // TODO: Heavy refactoring to make this more straightforward
                        var type = inheritingTypes[currentSelectedTypeIndex - CustomOptions.Count];
                        EditorGUI.ObjectField(position, unityObjectReference, type, GUIContent.none);
                        break;
                    case ReferenceOfType.Null:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                selectedPropertyDrawer.OnDraw(property, position, referenceType);
            }
            EditorGUI.indentLevel--;

            void SetType(int index)
            {
                selectedType.intValue = index;
                if (index < CustomOptions.Count)
                {
                    CustomOptions[index].Select(property);
                    return;
                }

                var newType = inheritingTypes[index - CustomOptions.Count];
                
                if (typeof(Object).IsAssignableFrom(newType))
                {
                    referenceProperty.managedReferenceValue = null;
                    typeReference.enumValueIndex = (int)ReferenceOfType.UnityObject;
                    unityObjectReference.objectReferenceValue = null;
                    return;
                }
                
                referenceProperty.managedReferenceValue = Activator.CreateInstance(newType);
                typeReference.enumValueIndex = (int)ReferenceOfType.SerializedReference;
                unityObjectReference.objectReferenceValue = null;
            }
        }
    }
}
*/
