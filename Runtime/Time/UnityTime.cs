using System;

namespace GatOR.Logic.Time
{
    public class UnityTime : IUpdateTime
    {
        public static UnityTime Instance { get; } = new UnityTime();

        public TimeSpan Time => TimeSpan.FromSeconds(UnityEngine.Time.timeAsDouble);
        public TimeSpan DeltaTime => TimeSpan.FromSeconds(UnityEngine.Time.deltaTime);
    }

    public class UnscaledUnityTime : IUpdateTime
    {
        public static UnscaledUnityTime Instance { get; } = new UnscaledUnityTime();

        public TimeSpan Time => TimeSpan.FromSeconds(UnityEngine.Time.timeAsDouble);
        public TimeSpan DeltaTime => TimeSpan.FromSeconds(UnityEngine.Time.deltaTime);
    }

    public class FixedUnityTime : IUpdateTime
    {
        public static FixedUnityTime Instance { get; } = new FixedUnityTime();

        public TimeSpan Time => TimeSpan.FromSeconds(UnityEngine.Time.fixedTimeAsDouble);
        public TimeSpan DeltaTime => TimeSpan.FromSeconds(UnityEngine.Time.fixedDeltaTime);
    }

    public class UnscaledFixedUnityTime : IUpdateTime
    {
        public static UnscaledFixedUnityTime Instance { get; } = new UnscaledFixedUnityTime();

        public TimeSpan Time => TimeSpan.FromSeconds(UnityEngine.Time.fixedUnscaledTimeAsDouble);
        public TimeSpan DeltaTime => TimeSpan.FromSeconds(UnityEngine.Time.fixedUnscaledDeltaTime);
    }    
}
