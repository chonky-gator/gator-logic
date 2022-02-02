using System;


namespace GatOR.Logic.Time
{
    /// <summary>
    /// Update delegate, using this instead of <see cref="Action{T}"/> to name and describe the argument.
    /// </summary>
    /// <param name="deltaTime">How much time has passed in this update.</param>
    public delegate void UpdateAction(TimeSpan deltaTime);

    /// <summary>
    /// Object that should notify when a update is called, with the delta time to
    /// tell how much time has passed since the last update.
    /// </summary>
    public interface IUpdateNotifier
    {
        event UpdateAction OnUpdate;
    }

    /// <summary>
    /// Interface that tells how much time has passed, ignoring dates. Created to use
    /// in most of our projects to mock time in tests and avoid using static <see cref="UnityEngine.Time.time"/>.
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// How much time has passed for this time frame.
        /// </summary>
        TimeSpan Time { get; }
    }

    public interface IUpdateTime : ITime
    {
        /// <summary>
        /// How much time has passed since the last update for this time frame.
        /// </summary>
        TimeSpan DeltaTime { get; }
    }
}
