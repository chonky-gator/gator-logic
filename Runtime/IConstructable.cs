
namespace GatOR.Logic
{
    public interface IConstructable
    {
        bool IsConstructed { get; }
    }

    public interface IConstructable<in T1> : IConstructable
    {
        void Construct(T1 arg1);
    }
    
    public interface IConstructable<in T1, in T2> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2);
    }

    public interface IConstructable<in T1, in T2, in T3> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IConstructable<in T1, in T2, in T3, in T4> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }

    public interface IConstructable<in T1, in T2, in T3, in T4, in T5> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }

    public static class ConstructableExtensions
    {
        public static void ThrowIfAlreadyConstructed(this IConstructable constructable)
        {
            if (constructable.IsConstructed)
                throw new AlreadyConstructedException();
        }

        public static void ThrowIfNotConstructed(this IConstructable constructable)
        {
            if (!constructable.IsConstructed)
                throw new NotConstructedException();
        }
    }
}
