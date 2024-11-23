#if PACKAGE_UNITASK
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using GatOR.Logic.Time;
using System;
using System.Collections.Generic;
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

    [Serializable]
    public class UniTaskUpdate : IUniTaskAsyncEnumerable<TimeSpan>
    {
        public static UniTaskUpdate Update { get; } = new UniTaskUpdate(PlayerLoopTiming.Update,
            UnityTime.Instance);
        public static UniTaskUpdate LateUpdate { get; } = new UniTaskUpdate(PlayerLoopTiming.FixedUpdate,
            FixedUnityTime.Instance);

        public PlayerLoopTiming Timing { get; }
        public IUpdateTime Time { get; }

        private IUniTaskAsyncEnumerable<TimeSpan> updates;
        
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

        public IUniTaskAsyncEnumerator<TimeSpan> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            updates ??= UniTaskAsyncEnumerable.EveryUpdate(Timing).Select(_ => Time.DeltaTime);
            return updates.GetAsyncEnumerator(cancellationToken);
        }
    }
}
#endif
