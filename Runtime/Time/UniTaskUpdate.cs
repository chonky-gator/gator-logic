#if PACKAGE_UNITASK
using Cysharp.Threading.Tasks;
using GatOR.Logic.Time;
using System;
using System.Threading;

namespace GatOR.Logic
{
    /// <summary>
    /// An object where we can get the updates with the delta time,
    /// useful to isolate unit tests and scale easily the time.
    /// </summary>
    public interface IUniTaskUpdate
    {
        UniTask<TimeSpan> Next(CancellationToken cancellationToken = default);
    }

    public class UniTaskUpdate
    {
        public static UniTaskUpdate Update { get; } = new UniTaskUpdate(PlayerLoopTiming.Update,
            UnityTime.Instance);
        public static UniTaskUpdate LateUpdate { get; } = new UniTaskUpdate(PlayerLoopTiming.FixedUpdate,
            FixedUnityTime.Instance);

        public PlayerLoopTiming Timing { get; }
        public IUpdateTime Time { get; }
        
        public UniTaskUpdate(PlayerLoopTiming timing, IUpdateTime time)
        {
            Timing = timing;
            Time = time;
        }

        public async UniTask<TimeSpan> Next(CancellationToken cancellationToken = default)
        {
            await UniTask.Yield(Timing, cancellationToken);
            return Time.DeltaTime;
        }
    }
}
#endif
