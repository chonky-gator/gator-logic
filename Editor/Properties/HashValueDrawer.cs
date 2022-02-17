using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GatOR.Logic.Editor.Properties
{
    public abstract class HashValueDrawer : PropertyDrawer
    {
        protected abstract string StringFieldName { get; }

        protected abstract int GetHash(string value);


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var nameProperty = property.FindPropertyRelative(StringFieldName);
            var displayName = property.displayName;
            var nameElement = new PropertyField(nameProperty, displayName);
            nameElement.RegisterCallback<SerializedPropertyChangeEvent>(OnChangeEvent);
            nameElement.RegisterCallback<GeometryChangedEvent>(OnChangeGeometryEvent);
            OnChangeNameID();
            return nameElement;

            void OnChangeEvent(SerializedPropertyChangeEvent evt) => OnChangeNameID();
            void OnChangeGeometryEvent(GeometryChangedEvent evt) => OnChangeNameID();

            void OnChangeNameID()
            {
                var label = nameElement.Q<Label>(className: "unity-property-field__label");
                if (label == null)
                    return;

                label.style.minWidth = default;
                const string className = "gator-extra__label";
                var hashLabel = label.parent.Q<Label>(className: className);
                if (hashLabel == null)
                {
                    hashLabel = new Label();
                    label.parent.Add(hashLabel);
                    hashLabel.PlaceInFront(label);
                    hashLabel.AddToClassList(className);

                    foreach (var cName in label.GetClasses())
                        hashLabel.AddToClassList(cName);

                    hashLabel.style.color = new Color(0.5f, 0.5f, 0.5f);
                    hashLabel.style.minWidth = 120;
                }

                int hash = GetHash(nameProperty.stringValue);
                hashLabel.text = $"(Hash: {hash})";
                nameElement.UnregisterCallback<GeometryChangedEvent>(OnChangeGeometryEvent);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameProperty = property.FindPropertyRelative(StringFieldName);
            label.text += $" (Hash: {GetHash(nameProperty.stringValue)})";
            EditorGUI.PropertyField(position, nameProperty, label);
        }
    }
}
