using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Properties
{
    public interface IReference<TReference>
        where TReference : class
    {
        TReference Interface { get; set; }
    }

    public enum ReferenceKind
    {
        Null,
        SerializedReference,
        UnityObject,
    }

    [Serializable]
    public struct ReferenceOf<TReference> : ISerializationCallbackReceiver
        where TReference : class
    {
        [SerializeField] internal Object unityObject;
        [SerializeReference] internal TReference serializedReference;
#if UNITY_EDITOR
        [SerializeField, HideInInspector] internal string selectedConcreteType;
#endif
        internal ReferenceKind kind;

        public ReferenceOf(TReference reference)
        {
            kind = default;
            unityObject = default;
            serializedReference = default;
#if UNITY_EDITOR
            selectedConcreteType = default;
#endif
            Value = reference;
        }

        public TReference Value
        {
            get => kind switch
            {
                ReferenceKind.Null => null,
                ReferenceKind.SerializedReference => serializedReference,
                ReferenceKind.UnityObject => serializedReference ??= unityObject as TReference,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                switch (value)
                {
                    case Object uObj:
                        serializedReference = value;
                        unityObject = uObj;
                        kind = ReferenceKind.UnityObject;
                        break;
                    case null:
                        unityObject = null;
                        serializedReference = null;
                        kind = ReferenceKind.Null;
                        break;
                    default:
                        unityObject = null;
                        serializedReference = value;
                        kind = ReferenceKind.SerializedReference;
                        break;
                }
            }
        }

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
                serializedReference = unityObject as TReference;
                kind = ReferenceKind.UnityObject;
            }
            else if (serializedReference != null)
            {
                unityObject = null;
                kind = ReferenceKind.SerializedReference;
            }
            else
            {
                kind = ReferenceKind.Null;
            }
        }
    }
}
