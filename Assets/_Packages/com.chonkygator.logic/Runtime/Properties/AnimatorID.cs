using UnityEngine;

namespace GatOR.Logic.Properties
{
    [System.Serializable]
    public struct AnimatorID : ISerializationCallbackReceiver
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

        public AnimatorID(string name) : this()
        {
            this.name = name;
        }

        public static int GetHash(string name) => Animator.StringToHash(name);

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            hasHash = false;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        public static implicit operator AnimatorID(string name) => new AnimatorID(name);
    }

    public static class AnimatorIDExtensions
    {
        public static void SetBool(this Animator animator, ref AnimatorID id, bool value)
        {
            animator.SetBool(id.Hash, value);
        }
    }
}
