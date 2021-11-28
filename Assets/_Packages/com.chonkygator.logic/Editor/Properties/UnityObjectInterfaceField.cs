using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Editor.Properties
{
    public class ObjectInterfaceField : BaseField<Object>
    {
        public ObjectInterfaceField(string label) : base(label, null)
        {
        }

        public static void DrawIMGUI(Rect position, SerializedProperty objectProperty, Type expectedType,
            Type unityObjectType = null)
        {
            unityObjectType ??= typeof(Object);

            var selectedObject = EditorGUI.ObjectField(position, objectProperty.objectReferenceValue,
                unityObjectType, objectProperty.serializedObject.targetObject);
            selectedObject = TryReadComponentFromGameObject(selectedObject, expectedType);
            if (selectedObject && !expectedType.IsAssignableFrom(selectedObject.GetType()))
                throw new InvalidCastException($"Selected object ({selectedObject.GetType()}) is not of type: {expectedType}");
            
            objectProperty.objectReferenceValue = selectedObject;
        }
        
        [Pure]
        private static Object TryReadComponentFromGameObject(Object obj, Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            
            if (obj is GameObject gameObject)
            {
                var component = gameObject.GetComponent(type);
                if (component)
                    return component;
            }
            return obj;
        }
    }
}
