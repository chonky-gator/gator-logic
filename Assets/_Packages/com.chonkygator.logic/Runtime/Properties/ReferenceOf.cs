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

    public enum ReferenceOfType
    {
        Null,
        SerializedReference,
        UnityObject,
    }

    internal static class ReferenceOf
    {
        public const string NameOfType = nameof(ReferenceOf<object>.type);
        public const string NameOfSerializedReference = nameof(ReferenceOf<object>.serializedReference);
        public const string NameOfUnityObject = nameof(ReferenceOf<object>.unityObject);
    }
    
    [Serializable]
    public struct ReferenceOf<TReference> : ISerializationCallbackReceiver
        where TReference : class
    {
        [SerializeField] internal ReferenceOfType type;
        [SerializeField] internal Object unityObject;
        [SerializeReference] internal TReference serializedReference;

        public TReference Reference
        {
            get => type switch
            {
                ReferenceOfType.Null => null,
                ReferenceOfType.SerializedReference => serializedReference,
                ReferenceOfType.UnityObject => serializedReference ??= unityObject as TReference,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                switch (value)
                {
                    case Object uObj:
                        serializedReference = value;
                        unityObject = uObj;
                        type = ReferenceOfType.UnityObject;
                        break;
                    default:
                        unityObject = null;
                        serializedReference = value;
                        type = ReferenceOfType.SerializedReference;
                        break;
                    case null:
                        unityObject = null;
                        serializedReference = null;
                        type = ReferenceOfType.Null;
                        break;
                }
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (serializedReference is Object)
                serializedReference = null;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Force cache
            _ = Reference;
        }
    }
}
