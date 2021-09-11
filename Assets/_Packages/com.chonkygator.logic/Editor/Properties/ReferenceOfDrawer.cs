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
            public string[] typeNames;
        }

        private static readonly Dictionary<Type, Cache> infoForTypes = new Dictionary<Type, Cache>();

        // This is to add custom options, not used at the moment
        private static readonly List<CustomReferenceOf> defaultOptions = new List<CustomReferenceOf>()
        {
            new NullCustomReferenceOf(),
        };

        public static float baseHeight;
        public static float referenceHeight;

        private static Cache GetOrCreateInfoForType(Type type)
        {
            if (infoForTypes.TryGetValue(type, out Cache cache))
            {
                // Debug.Log($"Got cache for type: {type}");
                return cache;
            }

            Type[] inheritingTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && type.IsAssignableFrom(t))
                .ToArray();
            string[] typeNames = defaultOptions
                .Select(option => option.Name)
                .Concat(inheritingTypes.Select(t => t.Name))
                .ToArray();
            var uniqueNameChecker = new HashSet<string>();
            foreach (string name in typeNames)
            {
                if (uniqueNameChecker.Contains(name))
                    throw new Exception($"Name \"{name}\" already exists.");

                uniqueNameChecker.Add(name);
            }

            Cache newCache = new Cache
            {
                inheritingTypes = inheritingTypes,
                typeNames = typeNames,
            };
            infoForTypes[type] = newCache;
            return newCache;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            baseHeight = base.GetPropertyHeight(property, label);
            var reference = property.FindPropertyRelative(nameof(ReferenceOf<object>.reference));
            referenceHeight = string.IsNullOrEmpty(reference.managedReferenceFullTypename) ? 0f
                : EditorGUI.GetPropertyHeight(reference, label);
            return baseHeight + referenceHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = baseHeight;
            SerializedProperty reference = property.FindPropertyRelative(nameof(ReferenceOf<object>.reference));

            Type currentReferenceType = EditorUtils.GetTypeWithFullName(reference.managedReferenceFullTypename);
            Type referenceType = EditorUtils.GetTypeWithFullName(reference.managedReferenceFieldTypename);

            var info = GetOrCreateInfoForType(referenceType);
            Type[] inheritingTypes = info.inheritingTypes;
            string[] typeNames = info.typeNames;

            int previousSelectedTypeIndex = GetSelectedIndex(currentReferenceType, inheritingTypes);
            int currentSelectedTypeIndex = EditorGUI.IntPopup(position, label.text, previousSelectedTypeIndex, typeNames, null);
            if (currentSelectedTypeIndex != previousSelectedTypeIndex)
                reference.managedReferenceValue = SetType(currentSelectedTypeIndex, referenceType, inheritingTypes);

            if (currentReferenceType != null)
            {
                EditorGUI.indentLevel++;
                position.y += position.height;
                EditorGUI.PropertyField(position, reference, true);
                EditorGUI.indentLevel--;
            }

            static int GetSelectedIndex(Type currentType, Type[] inheritingTypes)
            {
                for (int i = 0; i < defaultOptions.Count; i++)
                {
                    if (defaultOptions[i].IsType(currentType))
                        return i;
                }
                return Array.IndexOf(inheritingTypes, currentType) + defaultOptions.Count;
            }

            static object SetType(int index, Type fieldType, Type[] types)
            {
                if (index < defaultOptions.Count)
                    return defaultOptions[index].CreateInstrance(fieldType);

                Type newType = types[index - defaultOptions.Count];
                return Activator.CreateInstance(newType);
            }
        }
    }
}
