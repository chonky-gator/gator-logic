using System;
using GatOR.Logic.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;


namespace GatOR.Logic.Editor.Properties
{
    public class UnityObjectCustomReferenceOf : TypeCustomReferenceOf
    {
        public override GUIContent Name { get; } = new GUIContent("<Unity Object>");

        protected override ReferenceKind ExpectedType => ReferenceKind.UnityObject;

        public override bool IsSelected(SerializedProperty baseProperty, Type expectedType)
        {
            return false;
        }

        public override void Select(SerializedProperty baseProperty)
        {
            base.Select(baseProperty);
            baseProperty.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference)).managedReferenceValue = null;
        }

        public override float GetHeight(SerializedProperty baseProperty) => 16;

        public override void OnDraw(SerializedProperty baseProperty, Rect position, Type expectedType)
        {
            using var unityObjectProperty = baseProperty.FindPropertyRelative(nameof(ReferenceOf<object>.unityObject));
            ObjectInterfaceField.DrawIMGUI(position, unityObjectProperty, expectedType);
        }

        public override VisualElement CreateVisualElement(SerializedProperty baseProperty, Type expectedType)
        {
            return null;
        }
    }
}
