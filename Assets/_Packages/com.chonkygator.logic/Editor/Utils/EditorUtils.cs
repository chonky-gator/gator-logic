using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GatOR.Logic.Editor
{
    public static class EditorUtils
    {
        private static readonly char[] assemblySeparator = new char[] { ' ' };

        /// <summary>
        /// Gets the list or array type, or the same type.
        /// </summary>
        /// <param name="type">The field we want to infer the type from.</param>
        /// <returns>The individual type</returns>
        public static Type GetIndividualType(Type type)
        {
            // Array
            if (type.IsArray)
                return type.GetElementType();

            // List
            if (type.GetGenericTypeDefinition() == typeof(List<>))
                return type.GenericTypeArguments[0];

            return type;
        }

        /// <summary>
        /// Gets the list or array type, or the same type.
        /// </summary>
        /// <param name="field">The field we want to infer the type from.</param>
        /// <returns>The individual type</returns>
        public static Type GetIndividualType(FieldInfo field) => GetIndividualType(field.FieldType);

        public static Type GetTypeWithFullName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            string[] split = name.Split(assemblySeparator, 2);
            string assemblyName = split[0], typeName = split[1];

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.First(IsAssemblyWithName);
            var type = assembly.GetType(typeName);
            return type;

            bool IsAssemblyWithName(Assembly assembly)
            {
                var name = assembly.GetName();
                return name.Name == assemblyName;
            }
        }
    }
}
