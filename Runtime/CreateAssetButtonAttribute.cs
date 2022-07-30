using System;
using UnityEngine;

namespace GatOR.Logic
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CreateAssetButtonAttribute : PropertyAttribute
	{
	}
}
