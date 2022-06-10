
namespace GatOR.Logic
{
    public interface IConstructable
    {
        bool IsConstructed { get; }
    }

    public interface IConstructable<T1> : IConstructable
    {
        void Construct(T1 arg1);
    }
    
    public interface IConstructable<T1, T2> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2);
    }

    public interface IConstructable<T1, T2, T3> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IConstructable<T1, T2, T3, T4> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }

    public interface IConstructable<T1, T2, T3, T4, T5> : IConstructable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }

    public static class IConstructableExtensions
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
