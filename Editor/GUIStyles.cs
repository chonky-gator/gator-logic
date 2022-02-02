using UnityEditor;
using UnityEngine;


namespace GatOR.Logic.Editor
{
    public static class GUIStyles
    {
        private static GUIStyle labelStyle;
        public static GUIStyle RichTextLabelStyle => labelStyle ??= new GUIStyle(EditorStyles.label)
        {
            richText = true,
        };
    }
}
