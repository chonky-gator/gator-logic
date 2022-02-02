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
}
