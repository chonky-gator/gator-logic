using UnityEditor;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


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
                var elementFactory = GetVisualElementMethodFor(property) ?? Draw.Default;
                var element = elementFactory(property);
                if (element == null)
                    continue;

                inspector.Add(element);
            }

            return inspector;
        }

        protected abstract Func<SerializedProperty, VisualElement> GetVisualElementMethodFor(SerializedProperty property);


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

            public static VisualElement Hidden(SerializedProperty _) => null;
        }
    }
}
