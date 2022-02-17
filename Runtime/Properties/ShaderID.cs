using UnityEngine;


namespace GatOR.Logic.Properties
{
    [System.Serializable]
    public struct ShaderID : ISerializationCallbackReceiver
    {
        [SerializeField] internal string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                hasHash = false;
            }
        }

        // Not using nullable types to allow debug mode view in unity
        private bool hasHash;
        private int hash;
        public int Hash
        {
            get
            {
                if (!hasHash)
                {
                    hash = GetHash(name);
                    hasHash = true;
                }
                return hash;
            }
        }

        public ShaderID(string name) : this()
        {
            this.name = name;
        }

        public int CacheHash() => Hash;

        public static int GetHash(string name) => Shader.PropertyToID(name);

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            hasHash = false;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        public static implicit operator ShaderID(string name) => new ShaderID(name);
    }
}