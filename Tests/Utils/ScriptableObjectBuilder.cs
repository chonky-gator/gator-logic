using UnityEngine;

namespace GatOR.Logic.Tests.Utils
{
    public struct ScriptableObjectBuilder<T> : IBuilder<T> where T : ScriptableObject
    {
        private string withName;

        public ScriptableObjectBuilder<T> WithName(string name)
        {
            withName = name;
            return this;
        }
        
        public T Build() => UnityObjectMother.AScriptableObject<T>(withName);

        public static implicit operator T(ScriptableObjectBuilder<T> builder) => builder.Build();
    }
}
