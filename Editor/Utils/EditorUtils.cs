using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace GatOR.Logic.Editor
{
    public static class EditorUtils
    {
        private static readonly Regex AssemblyRegex = new Regex(@"([^\s]*)\s(.*)", RegexOptions.Compiled);

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

        /// <summary>
        /// Gets a type from the string format: "{Assembly name} {Type full namespaced name}".
        /// e.g: GatOR GatOR.ExampleClass
        /// </summary>
        /// <param name="name">The string with the required format.</param>
        /// <returns>The type or null if name was null.</returns>
        [CanBeNull]
        public static Type GetTypeWithFullName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var match = AssemblyRegex.Match(name);
            string assemblyName = match.Groups[1].Value, typeName = match.Groups[2].Value;

            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(IsAssemblyWithName);
            if (assembly == null)
                return null;
            
            var type = assembly.GetType(typeName);
            return type;

            bool IsAssemblyWithName(Assembly targetAssembly)
            {
                var nameInfo = targetAssembly.GetName();
                return nameInfo.Name == assemblyName;
            }
        }

        [NotNull]
        public static string AsFullnameType([NotNull] Type type)
        {
            return $"{type.Assembly.GetName().Name} {type.FullName}";
        }
    }
}
