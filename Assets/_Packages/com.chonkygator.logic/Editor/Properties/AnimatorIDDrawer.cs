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
            var displayName = property.displayName;
            var nameElement = new PropertyField(nameProperty, displayName);
            nameElement.RegisterCallback<SerializedPropertyChangeEvent>(OnChangeEvent);
            OnChangeNameID(nameProperty.stringValue);
            return nameElement;

            void OnChangeEvent(SerializedPropertyChangeEvent evt) => OnChangeNameID(evt.changedProperty.stringValue);
            
            void OnChangeNameID(string name)
            {
                var text = $"{displayName} (Hash: {Animator.StringToHash(name)})";
                nameElement.label = text;
                var label = nameElement.Q<Label>();
                if (label != null)
                    label.text = text;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameProperty = property.FindPropertyRelative(nameof(AnimatorID.name));
            label.text += $" (Hash: {Animator.StringToHash(nameProperty.stringValue)})";
            EditorGUI.PropertyField(position, nameProperty, label);
        }
    }
}
