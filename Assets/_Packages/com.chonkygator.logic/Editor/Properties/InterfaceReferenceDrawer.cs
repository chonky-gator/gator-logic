using GatOR.Logic.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Editor.Properties
{
    [CustomPropertyDrawer(typeof(InterfaceReference<,>), true)]
    public class InterfaceReferenceDrawer : PropertyDrawer
    {
        public const string ReferencePropName = InterfaceReference<object, Object>.ReferencePropName;
        public const string InterfacePropName = nameof(InterfaceReference<object, Object>.Interface);
        public const string ObjectPropName = nameof(InterfaceReference<object, Object>.Object);

        private float baseHeight;
        private GUIStyle labelStyle;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            baseHeight = base.GetPropertyHeight(property, label);
            return baseHeight * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            labelStyle ??= labelStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true,
            };

            // Determine the field type and get the interface to determine the generic arguments
            Type fieldType = property.propertyType switch
            {
                SerializedPropertyType.Generic => EditorUtils.GetIndividualType(fieldInfo),
                SerializedPropertyType.ManagedReference => EditorUtils.GetTypeWithFullName(property.managedReferenceFullTypename),
                _ => throw new NotImplementedException(property.propertyType.ToString()),
            };
            Type interfaceDefinitionType = fieldType.GetInterface(typeof(IUnityObjectInterfaceReference<,>).Name);
            Type[] genericArguments = interfaceDefinitionType.GetGenericArguments();
            Type interfaceType = genericArguments[0], objectType = genericArguments[1];

            SerializedProperty referenceProp = GetReferenceProperty(property);
            label.text += $" <color=#888888><interface {interfaceType.Name}></color>";
            // Tried to assign interfaceType too, but it won't search the component either when dragging a gameObject
            // TODO: Research if we can do this with VisualElements

            position.height = baseHeight;
            EditorGUI.LabelField(position, label, labelStyle);
            position.y += baseHeight;
            var newReference = EditorGUI.ObjectField(position, default(string), referenceProp.objectReferenceValue, objectType, true);

            SetProperty(property, interfaceType, newReference);
        }

        public static void SetProperty(SerializedProperty interfaceReferenceProp, Type expectedInterfaceType, Object setValue)
        {
            var referenceProp = GetReferenceProperty(interfaceReferenceProp);

            if (TryGetObjectWithInterface(out Object obj))
            {
                referenceProp.objectReferenceValue = obj;
                // interfaceReferenceProp.prefabOverride = true;
            }

            // True means it should modify the current value
            bool TryGetObjectWithInterface(out Object obj)
            {
                switch (setValue)
                {
                    case GameObject setGo:
                        bool result = setGo.TryGetComponent(expectedInterfaceType, out Component component);
                        obj = component;
                        return result;
                    case Component setComponent:
                        if (expectedInterfaceType.IsAssignableFrom(setComponent.GetType()) || setComponent.TryGetComponent(expectedInterfaceType, out setComponent))
                        {
                            obj = setComponent;
                            return true;
                        }
                        else
                        {
                            obj = null;
                            return false;
                        }
                    case null:
                        obj = null;
                        return true;
                    default:
                        if (expectedInterfaceType.IsAssignableFrom(setValue.GetType()))
                        {
                            obj = setValue;
                            return true;
                        }
                        else
                        {
                            obj = null;
                            return false;
                        }
                }
            }
        }

        public static SerializedProperty GetReferenceProperty(SerializedProperty objectProperty) =>
            objectProperty.FindPropertyRelative(InterfaceReference<object, Object>.ReferencePropName);


        public static bool TryGetInterface(Object obj, Type interfaceType, out Object objectWithInterface)
        {
            bool result;
            switch (obj)
            {
                case GameObject gameObject:
                    result = gameObject.TryGetComponent(interfaceType, out Component goComponent);
                    objectWithInterface = goComponent;
                    return result;
                case Component component:
                    result = component.TryGetComponent(interfaceType, out component);
                    objectWithInterface = component;
                    return result;
                default:
                    if (interfaceType.IsAssignableFrom(obj.GetType()))
                    {
                        objectWithInterface = obj;
                        return true;
                    }
                    else
                    {
                        objectWithInterface = null;
                        return false;
                    }
            }
        }
    }
}
