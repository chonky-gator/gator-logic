using UnityEditor;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace GatOR.Logic.Editor
{
    public abstract class CustomUIElementsEditor : UnityEditor.Editor
    {
        protected virtual bool ShouldShowMonoScript => true;


        public override VisualElement CreateInspectorGUI()
        {
            var inspector = new VisualElement();

            var property = serializedObject.GetIterator();
            property.NextVisible(true);
            if (ShouldShowMonoScript)
            {
                var monoScriptField = new PropertyField(property);
                inspector.Add(monoScriptField);
            }

            while (property.NextVisible(false))
            {
                var element = GetVisualElementFor(property) ?? Draw.Default(property);
                if (element == null)
                    continue;

                inspector.Add(element);
            }

            return inspector;
        }

        protected abstract VisualElement GetVisualElementFor(SerializedProperty property);


        public class Draw
        {
            public static VisualElement Default(SerializedProperty property)
            {
                return new PropertyField(property, property.displayName);
            }

            public static VisualElement Disabled(SerializedProperty property)
            {
                var element = Default(property);
                element.SetEnabled(false);
                return element;
            }

            public static VisualElement Hidden(SerializedProperty property)
            {
                var element = Default(property);
                element.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                return element;
            }

            public static VisualElement If(SerializedProperty property, SerializedProperty boolCondition,
                long updateIntervalMs = 100)
            {
                return If(property, () => boolCondition.boolValue, updateIntervalMs);
            }

            public static VisualElement If(SerializedProperty property, Func<bool> boolCondition,
                long updateIntervalMs = 100)
            {
                var element = Default(property);
                UpdateDisplay();
                element.schedule.Execute(UpdateDisplay).Every(updateIntervalMs);
                return element;

                void UpdateDisplay()
                {
                    element.style.display = boolCondition() ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex)
                        : new StyleEnum<DisplayStyle>(DisplayStyle.None);
                }
            }
        }
    }
}
