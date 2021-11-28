using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using GatOR.Logic.Properties;
using System.Collections.Generic;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(ReferenceOf<>))]
    public class ReferenceOfDrawer : PropertyDrawer
    {
        private struct Cache
        {
            public Type[] inheritingTypes;
            public GUIContent[] typeNames;
        }

        private static readonly Dictionary<Type, Cache> InfoForTypes = new Dictionary<Type, Cache>();

        // This is to add custom options, not used at the moment
        private static readonly List<ICustomReferenceOf> CustomOptions = new List<ICustomReferenceOf>()
        {
            new NullCustomReferenceOf(),
            new UnityObjectCustomReferenceOf(),
        };

        private static float _baseHeight;

        private int selectedPropertyDrawerIndex = -1;
        private ICustomReferenceOf SelectedPropertyDrawer => (selectedPropertyDrawerIndex >= 0)
            ? CustomOptions[selectedPropertyDrawerIndex] : null;

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
                .Where(t => !t.IsAbstract && type.IsAssignableFrom(t) && !typeof(UnityEngine.Object).IsAssignableFrom(t))
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
            selectedPropertyDrawerIndex = CustomOptions.FindIndex(x => x.IsSelected(property, referenceType));
            
            _baseHeight = base.GetPropertyHeight(property, label);
            var referenceHeight = SelectedPropertyDrawer?.GetHeight(property) ?? EditorGUI.GetPropertyHeight(referenceProperty, label);
            return _baseHeight + referenceHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var referenceProperty = property.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference));

            Type currentReferenceType = EditorUtils.GetTypeWithFullName(referenceProperty.managedReferenceFullTypename);
            Type referenceType = EditorUtils.GetTypeWithFullName(referenceProperty.managedReferenceFieldTypename);

            var info = GetOrCreateInfoForType(referenceType);
            Type[] inheritingTypes = info.inheritingTypes;
            GUIContent[] typeNames = info.typeNames;

            int previousSelectedTypeIndex = GetSelectedIndex();
            label.text += $" <color=#888888><{referenceType.Name}> type</color>";

            position.height = _baseHeight;
            Rect labelPosition = position;
            labelPosition.width *= 0.5f;
            EditorGUI.LabelField(labelPosition, label, GUIStyles.RichTextLabelStyle);

            labelPosition.x += labelPosition.width;
            int currentSelectedTypeIndex = EditorGUI.IntPopup(labelPosition, null, previousSelectedTypeIndex, typeNames, null);
            if (currentSelectedTypeIndex != previousSelectedTypeIndex)
                SetType(currentSelectedTypeIndex);

            EditorGUI.indentLevel++;
            position.y += position.height;
            if (SelectedPropertyDrawer == null)
                EditorGUI.PropertyField(position, referenceProperty, true);
            else
                SelectedPropertyDrawer.OnDraw(property, position, referenceType);
            EditorGUI.indentLevel--;

            int GetSelectedIndex()
            {
                if (selectedPropertyDrawerIndex >= 0)
                    return selectedPropertyDrawerIndex;
                
                return Array.IndexOf(inheritingTypes, currentReferenceType) + CustomOptions.Count;
            }

            void SetType(int index)
            {
                if (index < CustomOptions.Count)
                {
                    CustomOptions[index].Select(property);
                    return;
                }
                var newType = inheritingTypes[index - CustomOptions.Count];
                referenceProperty.managedReferenceValue = Activator.CreateInstance(newType);
                
                using var typeReference = property.FindPropertyRelative(nameof(ReferenceOf<object>.type));
                typeReference.enumValueIndex = (int)ReferenceOfType.SerializedReference;
                
                using var unityObjectReference = property.FindPropertyRelative(nameof(ReferenceOf<object>.unityObject));
                unityObjectReference.objectReferenceValue = null;
            }
        }
    }
}
