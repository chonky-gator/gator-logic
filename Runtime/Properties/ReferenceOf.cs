using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Properties
{
    public enum ReferenceKind
    {
        Null,
        SerializedReference,
        UnityObject,
    }

    /// <summary>
    /// Use this struct to reference to any unity object or serializable object of type T.
    /// Useful to reference interfaces to abstract logic and to facilitate unit testing.
    /// </summary>
    /// <typeparam name="T">The type we want to reference</typeparam>
    [Serializable]
    public struct ReferenceOf<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField] internal Object unityObject;
        [SerializeReference] internal T serializedReference;
#if UNITY_EDITOR
        [SerializeField, HideInInspector] internal string selectedConcreteType;
#endif

        /// <summary>
        /// Are we referencing an unity object, a serialized object or just a null reference.
        /// </summary>
        public ReferenceKind Kind { get; private set; }

        public ReferenceOf(T reference)
        {
            Kind = default;
            unityObject = default;
            serializedReference = default;
#if UNITY_EDITOR
            selectedConcreteType = default;
#endif
            Value = reference;
        }

        public T Value
        {
            get => Kind switch
            {
                ReferenceKind.Null => null,
                ReferenceKind.SerializedReference => serializedReference,
                ReferenceKind.UnityObject => serializedReference ??= unityObject as T,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                switch (value)
                {
                    case Object uObj:
                        serializedReference = value;
                        unityObject = uObj;
                        Kind = ReferenceKind.UnityObject;
                        break;
                    case null:
                        unityObject = null;
                        serializedReference = null;
                        Kind = ReferenceKind.Null;
                        break;
                    default:
                        unityObject = null;
                        serializedReference = value;
                        Kind = ReferenceKind.SerializedReference;
                        break;
                }
            }
        }

        public override string ToString() => $"Reference<{typeof(T).FullName}> {Value}";

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Value is Object)
            {
                serializedReference = null;
            }
            else
            {
                unityObject = null;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (unityObject != null)
            {
                serializedReference = unityObject as T;
                Kind = ReferenceKind.UnityObject;
            }
            else if (serializedReference != null)
            {
                unityObject = null;
                Kind = ReferenceKind.SerializedReference;
            }
            else
            {
                Kind = ReferenceKind.Null;
            }
        }
    }
}
