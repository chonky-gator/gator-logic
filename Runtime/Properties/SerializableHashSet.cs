using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GatOR.Logic
{
    [System.Serializable]
    public class SerializableHashSet<T> : ISet<T>, ISerializationCallbackReceiver
    {
        [SerializeField, Delayed] internal T[] serializedArray;
        [SerializeField] internal bool conflict;

        private HashSet<T> hashSet;

        #region Constructors
        public SerializableHashSet()
        {
            hashSet = new HashSet<T>();
        }
        
        public SerializableHashSet(IEnumerable<T> collection)
        {
            hashSet = new HashSet<T>(collection);
        }
        
        public SerializableHashSet(IEqualityComparer<T> comparer)
        {
            hashSet = new HashSet<T>(comparer);
        }
        
        public SerializableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            hashSet = new HashSet<T>(collection, comparer);
        }
        
        public SerializableHashSet(int capacity)
        {
            hashSet = new HashSet<T>(capacity);
        }
        
        public SerializableHashSet(int capacity, IEqualityComparer<T> comparer)
        {
            hashSet = new HashSet<T>(capacity, comparer);
        }
        #endregion

        #region ISerializationCallbackReceiver
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            serializedArray ??= hashSet.ToArray();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            conflict = false;
            
            hashSet ??= serializedArray != null ? new HashSet<T>(serializedArray.Length) : new HashSet<T>();
            hashSet.Clear();
            
            if (serializedArray == null)
                return;

            foreach (var item in serializedArray)
            {
                bool result = hashSet.Add(item);
                conflict |= !result;
            }
            
            if (!conflict)
                serializedArray = null;
        }
        #endregion

        #region ISet<T>
        public IEnumerator<T> GetEnumerator()
        {
            return hashSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) hashSet).GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            hashSet.Add(item);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            hashSet.ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            hashSet.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return hashSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return hashSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return hashSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return hashSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return hashSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return hashSet.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            hashSet.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            hashSet.UnionWith(other);
        }

        bool ISet<T>.Add(T item)
        {
            return hashSet.Add(item);
        }

        public void Clear()
        {
            hashSet.Clear();
        }

        public bool Contains(T item)
        {
            return hashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            hashSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return hashSet.Remove(item);
        }

        public int Count => hashSet.Count;

        bool ICollection<T>.IsReadOnly => ((ICollection<T>)hashSet).IsReadOnly;
        #endregion
    }
}
