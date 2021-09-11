using System;


namespace GatOR.Logic.Editor.Properties
{
    public abstract class CustomReferenceOf
    {
        public abstract string Name { get; }
        public abstract object CreateInstrance(Type expectedType);
        public abstract bool IsType(Type type);
    }

    public class NullCustomReferenceOf : CustomReferenceOf
    {
        public override string Name => "<None>";
        public override object CreateInstrance(Type expectedType) => null;
        public override bool IsType(Type type) => type == null;
    }
}
