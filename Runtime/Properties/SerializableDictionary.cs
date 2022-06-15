using System.Collections.Generic;
using UnityEngine;

namespace GatOR.Logic
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #if UNITY_EDITOR
        [SerializeField] internal TKey add;
        #endif
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
            if (kvps != null)
                return;
            
            kvps = new SerializableKeyValuePair<TKey, TValue>[Count];
            int i = 0;
            foreach (var kvp in this)
            {
                kvps[i] = new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
                i++;
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
