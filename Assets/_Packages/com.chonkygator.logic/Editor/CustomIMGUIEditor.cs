using UnityEditor;
using System;


namespace GatOR.Logic.Editor
{
    public abstract class CustomIMGUIEditor : UnityEditor.Editor
    {
        public delegate void DrawPropertyAction(SerializedProperty property);

        protected virtual bool ShouldShowMonoScript => true;

        public override void OnInspectorGUI()
        {
            var property = serializedObject.GetIterator();
            property.NextVisible(true);
            if (ShouldShowMonoScript)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(property);
                EditorGUI.EndDisabledGroup();
            }

            while (property.NextVisible(false))
            {
                DrawPropertyAction drawAction = GetDrawActionFor(property) ?? Draw.Default;
                drawAction(property);
            }
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract DrawPropertyAction GetDrawActionFor(SerializedProperty property);


        /// <summary>
        /// Collection of <see cref="DrawPropertyAction"/> delegates, non-static to inherit and extend different draw modes.
        /// E.g: public new class Draw { }
        /// </summary>
        public class Draw
        {
            public static DrawPropertyAction Default => (SerializedProperty property) =>
            {
                EditorGUILayout.PropertyField(property);
            };

            public static DrawPropertyAction If(bool condition) => condition ? default(DrawPropertyAction) : Hidden;


            public static DrawPropertyAction Disabled => (SerializedProperty property) =>
            {
                EditorGUI.BeginDisabledGroup(true);
                Default(property);
                EditorGUI.EndDisabledGroup();
            };

            public static DrawPropertyAction DisabledIf(bool condition, DrawPropertyAction ifNot = null) =>
                condition ? Disabled : ifNot;


            public static DrawPropertyAction HiddenIf(bool condition, DrawPropertyAction ifNot = null) =>
                condition ? Hidden : ifNot;

            public static void Hidden(SerializedProperty _)
            {
            }
        }
    }
}
