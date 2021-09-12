using UnityEngine;

namespace GatOR.Logic.Properties
{
    [System.Serializable]
    public struct ReferenceOf<T>
    {
        [SerializeReference] internal T reference;
        public T Reference { get => reference; set => reference = value; }
    }
}
