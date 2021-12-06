using System;
using System.Reflection;
using GatOR.Logic.Properties;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Editor
{
	[CustomEditor(typeof(Object), true, isFallback = true)]
	public class UIElementsEditor : CustomUIElementsEditor
	{
		private UseUIElementsEditorAttribute attribute;
		protected override bool ShouldShowMonoScript => attribute.ShowMonoscript;

		public override VisualElement CreateInspectorGUI()
		{
			attribute = target.GetType().GetCustomAttribute<UseUIElementsEditorAttribute>();
			return attribute != null ? base.CreateInspectorGUI() : null;
		}

		protected override Func<SerializedProperty, VisualElement>
			GetVisualElementMethodFor(SerializedProperty property)
		{
			return null;
		}
	}
}