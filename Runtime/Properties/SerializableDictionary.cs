using System.Collections.Generic;
using UnityEngine;

namespace GatOR.Logic
{
    internal class SerializableDictionaryNames
    {
        public const string ToAdd = nameof(SerializableDictionary<object, object>.add);
        public const string Kvps = nameof(SerializableDictionary<object, object>.kvps);
        public const string Conflict = nameof(SerializableDictionary<object, object>.conflict);
    }

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] internal TKey add;
        [SerializeField] internal SerializableKeyValuePair<TKey, TValue>[] kvps;
        [SerializeField] internal bool conflict;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            conflict = false;
            foreach (var kvp in kvps)
            {
                conflict |= ContainsKey(kvp.key);
                this[kvp.key] = kvp.value;
            }

            if (!conflict)
            {
                kvps = null;
                // Debug.Log("Succesfully deserialized dictionary!");
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (kvps == null)
            {
                kvps = new SerializableKeyValuePair<TKey, TValue>[Count];
                int i = 0;
                foreach (var kvp in this)
                {
                    kvps[i] = new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
                    i++;
                }
            }
        }
    }

    [System.Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        [Delayed] public TKey key;
        public TValue value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
