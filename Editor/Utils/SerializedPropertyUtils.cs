using System;
using UnityEditor;

namespace GatOR.Logic.Editor
{
	public static class SerializedPropertyUtils
	{
		public static void CopyValueFrom(this SerializedProperty target, SerializedProperty source) =>
			CopyValueTo(source, target);

		public static void CopyValueTo(this SerializedProperty source, SerializedProperty target)
		{
			if (source.propertyType != target.propertyType)
				throw new ArgumentException("Source and target must have the same property type");

			switch (source.propertyType)
			{
				case SerializedPropertyType.ObjectReference:
					target.objectReferenceValue = source.objectReferenceValue;
					break;
				case SerializedPropertyType.Integer:
				case SerializedPropertyType.LayerMask:
				case SerializedPropertyType.Character:
					target.intValue = source.intValue;
					break;
				case SerializedPropertyType.Boolean:
					target.boolValue = source.boolValue;
					break;
				case SerializedPropertyType.Float:
					target.floatValue = source.floatValue;
					break;
				case SerializedPropertyType.String:
					target.stringValue = source.stringValue;
					break;
				case SerializedPropertyType.Color:
					target.colorValue = source.colorValue;
					break;
				case SerializedPropertyType.Enum:
					target.enumValueIndex = source.enumValueIndex;
					break;
				case SerializedPropertyType.Vector2:
					target.vector2Value = source.vector2Value;
					break;
				case SerializedPropertyType.Vector3:
					target.vector3Value = source.vector3Value;
					break;
				case SerializedPropertyType.Vector4:
					target.vector4Value = source.vector4Value;
					break;
				case SerializedPropertyType.Rect:
					target.rectValue = source.rectValue;
					break;
				case SerializedPropertyType.ArraySize:
					target.arraySize = source.arraySize;
					break;
				case SerializedPropertyType.AnimationCurve:
					target.animationCurveValue = source.animationCurveValue;
					break;
				case SerializedPropertyType.Bounds:
					target.boundsValue = source.boundsValue;
					break;
				case SerializedPropertyType.Quaternion:
					target.quaternionValue = source.quaternionValue;
					break;
				case SerializedPropertyType.Vector2Int:
					target.vector2IntValue = source.vector2IntValue;
					break;
				case SerializedPropertyType.Vector3Int:
					target.vector3IntValue = source.vector3IntValue;
					break;
				case SerializedPropertyType.RectInt:
					target.rectIntValue = source.rectIntValue;
					break;
				case SerializedPropertyType.BoundsInt:
					target.boundsValue = source.boundsValue;
					break;
				case SerializedPropertyType.ManagedReference:
					target.managedReferenceValue = source.managedReferenceValue;
					break;
				case SerializedPropertyType.Hash128:
					target.hash128Value = source.hash128Value;
					break;
				case SerializedPropertyType.FixedBufferSize:
				case SerializedPropertyType.ExposedReference:
				case SerializedPropertyType.Gradient:
				case SerializedPropertyType.Generic:
				default:
					throw new NotImplementedException(source.propertyType.ToString());
			}
		}

		public static void ClearValue(this SerializedProperty prop)
		{
			switch (prop.propertyType)
			{
				case SerializedPropertyType.ObjectReference:
					prop.objectReferenceValue = null;
					break;
				case SerializedPropertyType.Integer:
				case SerializedPropertyType.LayerMask:
				case SerializedPropertyType.Character:
					prop.intValue = 0;
					break;
				case SerializedPropertyType.Boolean:
					prop.boolValue = default;
					break;
				case SerializedPropertyType.Float:
					prop.floatValue = default;
					break;
				case SerializedPropertyType.String:
					prop.stringValue = default;
					break;
				case SerializedPropertyType.Color:
					prop.colorValue = default;
					break;
				case SerializedPropertyType.Enum:
					prop.enumValueIndex = default;
					break;
				case SerializedPropertyType.Vector2:
					prop.vector2Value = default;
					break;
				case SerializedPropertyType.Vector3:
					prop.vector3Value = default;
					break;
				case SerializedPropertyType.Vector4:
					prop.vector4Value = default;
					break;
				case SerializedPropertyType.Rect:
					prop.rectValue = default;
					break;
				case SerializedPropertyType.ArraySize:
					prop.arraySize = default;
					break;
				case SerializedPropertyType.AnimationCurve:
					prop.animationCurveValue = default;
					break;
				case SerializedPropertyType.Bounds:
					prop.boundsValue = default;
					break;
				case SerializedPropertyType.Quaternion:
					prop.quaternionValue = default;
					break;
				case SerializedPropertyType.Vector2Int:
					prop.vector2IntValue = default;
					break;
				case SerializedPropertyType.Vector3Int:
					prop.vector3IntValue = default;
					break;
				case SerializedPropertyType.RectInt:
					prop.rectValue = default;
					break;
				case SerializedPropertyType.BoundsInt:
					prop.boundsIntValue = default;
					break;
				case SerializedPropertyType.ManagedReference:
					prop.managedReferenceValue = default;
					break;
				case SerializedPropertyType.Hash128:
					prop.hash128Value = default;
					break;
				case SerializedPropertyType.FixedBufferSize:
				case SerializedPropertyType.ExposedReference:
				case SerializedPropertyType.Gradient:
				case SerializedPropertyType.Generic:
				default:
					throw new NotImplementedException(prop.propertyType.ToString());
			}
		}
	}
}