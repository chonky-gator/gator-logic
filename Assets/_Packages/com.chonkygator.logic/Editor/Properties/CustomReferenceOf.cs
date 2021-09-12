using System;
using UnityEngine;

namespace GatOR.Logic.Editor.Properties
{
    public abstract class CustomReferenceOf
    {
        public abstract GUIContent Name { get; }
        public abstract object CreateInstrance(Type expectedType);
        public abstract bool IsType(Type type);
    }

    public class NullCustomReferenceOf : CustomReferenceOf
    {
        public override GUIContent Name { get; } = new GUIContent("<None>");
        public override object CreateInstrance(Type expectedType) => null;
        public override bool IsType(Type type) => type == null;
    }
}
