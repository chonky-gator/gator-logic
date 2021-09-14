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

        public int CacheHash() => Hash;

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
        public static bool GetBool(this Animator animator, ref AnimatorID id) => animator.GetBool(id.Hash);

        public static void SetBool(this Animator animator, ref AnimatorID id, bool value)
        {
            animator.SetBool(id.Hash, value);
        }


        public static float GetFloat(this Animator animator, ref AnimatorID id) => animator.GetFloat(id.Hash);

        public static void SetFloat(this Animator animator, ref AnimatorID id, float value)
        {
            animator.SetFloat(id.Hash, value);
        }


        public static int GetInteger(this Animator animator, ref AnimatorID id) => animator.GetInteger(id.Hash);

        public static void SetInteger(this Animator animator, ref AnimatorID id, int value)
        {
            animator.SetInteger(id.Hash, value);
        }


        public static void ResetTrigger(this Animator animator, ref AnimatorID id) => animator.ResetTrigger(id.Hash);

        public static void SetTrigger(this Animator animator, ref AnimatorID id) => animator.SetTrigger(id.Hash);


        public static bool IsParameterControlledByCurve(this Animator animator, ref AnimatorID id) =>
            animator.IsParameterControlledByCurve(id.Hash);
    }
}
