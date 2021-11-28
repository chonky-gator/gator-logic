using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace GatOR.Logic.Properties
{
    public interface IUnityObjectInterfaceReference<TInterface, TObject> : IReference<TInterface>
        where TInterface : class
        where TObject : Object
    {
        public TObject Object { get; }
    }

    [Serializable]
    public class InterfaceReference<TInterface, TObject> :
        IUnityObjectInterfaceReference<TInterface, TObject>,
        ISerializationCallbackReceiver,
        IEquatable<TObject>
        where TInterface : class
        where TObject : Object
    {
        internal const string ReferencePropName = nameof(m_object);

        [SerializeField] private TObject m_object;
        public TObject Object => m_object;

        private TInterface _interface;
        public TInterface Interface
        {
            get => _interface;
            set
            {
                _interface = value;
                m_object = value as TObject;
            }
        }

        public bool IsComponent => Object is Component;
        public bool HasValue => _interface != null;


        public InterfaceReference()
        {
        }

        public InterfaceReference(TInterface @interface)
        {
            _interface = @interface;
        }


        public override bool Equals(object obj) => Equals(obj as TObject);

        public bool Equals(TObject other)
        {
            return Object == other;
        }

        public override int GetHashCode()
        {
            if (Interface == null)
                return 0;

            return Interface.GetHashCode();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_object is TInterface interfaceReference)
                _interface = interfaceReference;
            else
                m_object = null;
        }

        public static implicit operator TInterface(InterfaceReference<TInterface, TObject> reference) => reference.Interface;
        public static implicit operator TObject(InterfaceReference<TInterface, TObject> reference) => reference.Object;
    }

    [Serializable]
    public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object>
        where TInterface : class
    {
    }

    [Serializable]
    public class InterfaceComponent<TInterface> : InterfaceReference<TInterface, Component>
        where TInterface : class
    {
    }
}
