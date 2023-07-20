using System;
using GatOR.Logic.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Editor.Properties
{
    public interface ICustomReferenceOf
    {
        GUIContent Name { get; }
        
        bool IsSelected(SerializedProperty baseProperty, Type expectedType);
        void Select(SerializedProperty baseProperty);
        
        float GetHeight(SerializedProperty baseProperty);
        void OnDraw(SerializedProperty baseProperty, Rect position, Type expectedType);
        VisualElement CreateVisualElement(SerializedProperty baseProperty, Type expectedType);
    }

    public abstract class TypeCustomReferenceOf : ICustomReferenceOf
    {
        public abstract GUIContent Name { get; }
        
        protected abstract ReferenceKind ExpectedType { get; }
        
        public virtual bool IsSelected(SerializedProperty baseProperty, Type expectedType)
        {
            using var typeProperty = baseProperty.FindPropertyRelative(nameof(ReferenceOf<object>.kind));
            return (ReferenceKind)typeProperty.enumValueIndex == ExpectedType;
        }

        public virtual void Select(SerializedProperty baseProperty)
        {
            using var typeProperty = baseProperty.FindPropertyRelative(nameof(ReferenceOf<object>.kind));
            typeProperty.enumValueIndex = (int)ExpectedType;
        }

        public abstract float GetHeight(SerializedProperty baseProperty);
        public abstract void OnDraw(SerializedProperty baseProperty, Rect position, Type expectedType);
        public abstract VisualElement CreateVisualElement(SerializedProperty baseProperty, Type expectedType);
    }

    public class NullCustomReferenceOf : TypeCustomReferenceOf
    {
        public override GUIContent Name { get; } = new GUIContent("<None>");

        protected override ReferenceKind ExpectedType => ReferenceKind.Null;

        public override void Select(SerializedProperty baseProperty)
        {
            base.Select(baseProperty);
            baseProperty.FindPropertyRelative(nameof(ReferenceOf<object>.serializedReference)).managedReferenceValue = null;
            baseProperty.FindPropertyRelative(nameof(ReferenceOf<object>.unityObject)).objectReferenceValue = null;
        }

        public override float GetHeight(SerializedProperty baseProperty) => 0;
        public override void OnDraw(SerializedProperty baseProperty, Rect position, Type expectedType) { }
        public override VisualElement CreateVisualElement(SerializedProperty baseProperty, Type expectedType) => null;
    }
}
