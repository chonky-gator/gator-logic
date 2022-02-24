using System;
using System.Reflection;

namespace GatOR.Logic.Tests.Utils
{
    public class ReflectionBuilder<T>
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        private readonly object obj;
        public T Object => (T)obj;

        public ReflectionBuilder()
        {
            obj = Activator.CreateInstance(typeof(T));
        }


        public FieldInfo GetField(string name) => typeof(T).GetField(name, Flags);

        public ReflectionBuilder<T> Set(string name, object value)
        {
            GetField(name).SetValue(obj, value);
            return this;
        }

        public ReflectionBuilder<T> SetReference(string name, UnityEngine.Object reference) =>
            Set(name, reference ? reference.GetInstanceID() : 0);
    }
}
