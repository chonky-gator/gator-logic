using GatOR.Logic.Properties;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(AnimatorID))]
    public class AnimatorIDDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var nameProperty = property.FindPropertyRelative(nameof(AnimatorID.name));
            return new PropertyField(nameProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameProperty = property.FindPropertyRelative(nameof(AnimatorID.name));
            label.text += $" (Hash: {Animator.StringToHash(nameProperty.stringValue)})";
            EditorGUI.PropertyField(position, nameProperty, label);
        }
    }
}
