using System;

namespace GatOR.Logic.Properties
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class UseUIElementsEditorAttribute : Attribute
	{
		public bool ShowMonoscript { get; set; } = true;
		
		public UseUIElementsEditorAttribute()
		{
		}
	}
}