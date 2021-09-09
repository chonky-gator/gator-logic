using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using GatOR.Logic.Properties;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(ReferenceOf<>))]
    public class ReferenceOfDrawer : PropertyDrawer
    {
        public static float baseHeight;
        public static float referenceHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            baseHeight = base.GetPropertyHeight(property, label);
            var reference = property.FindPropertyRelative("reference");
            referenceHeight = string.IsNullOrEmpty(reference.managedReferenceFullTypename) ? 0f
                : EditorGUI.GetPropertyHeight(reference, label);
            return baseHeight + referenceHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = baseHeight;
            var reference = property.FindPropertyRelative("reference");
            Type currentReferenceType = EditorUtils.GetTypeWithFullName(reference.managedReferenceFullTypename);

            Type fieldType = EditorUtils.GetIndividualType(fieldInfo);
            Type referenceType = fieldType.GetGenericArguments()[0];

            // TODO: We should cache these
            Type[] inheritingTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && referenceType.IsAssignableFrom(t))
                .ToArray();
            string[] typeNames = Enumerable.Repeat("<None>", 1).Concat(inheritingTypes.Select(t => t.Name)).ToArray();
            int previousSelectedTypeIndex = Array.IndexOf(inheritingTypes, currentReferenceType) + 1;

            // EditorGUI.DropdownButton(position, new GUIContent("Test"), FocusType.Keyboard);
            // Debug.Log(reference.managedReferenceFullTypename);
            int currentSelectedTypeIndex = EditorGUI.IntPopup(position, label.text, previousSelectedTypeIndex, typeNames, null);
            if (currentSelectedTypeIndex != previousSelectedTypeIndex)
                SetType(reference, currentSelectedTypeIndex, inheritingTypes);

            EditorGUI.indentLevel++;
            position.y += position.height;
            EditorGUI.PropertyField(position, reference, true);
            EditorGUI.indentLevel--;

            static void SetType(SerializedProperty reference, int index, Type[] types)
            {
                if (index <= 0)
                {
                    reference.managedReferenceValue = null;
                    return;
                }

                Type newType = types[index - 1];
                reference.managedReferenceValue = Activator.CreateInstance(newType);
            }
        }
    }
}
