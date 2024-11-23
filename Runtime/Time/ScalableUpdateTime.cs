using System;

namespace GatOR.Logic.Time
{
    [Serializable]
    public class ScalableUpdateTime : IUpdateTime, IUpdateNotifier, IDisposable
    {
        private readonly IUpdateNotifier updateNotifier;
        private bool disposed;

        private TimeSpan currentTime;
        public TimeSpan Time => currentTime;
        public TimeSpan DeltaTime { get; set; }

        public double Scale { get; set; } = 1f;

        public event UpdateAction OnUpdate;


        public ScalableUpdateTime(IUpdateNotifier updateNotifier)
        {
            this.updateNotifier = updateNotifier ?? throw new ArgumentNullException(nameof(updateNotifier));
            updateNotifier.OnUpdate += OnUpdateNotified;
        }


        private void OnUpdateNotified(TimeSpan deltaTime)
        {
            var scaledDeltaTime = new TimeSpan((long)(deltaTime.Ticks * Scale));
            currentTime += scaledDeltaTime;
            DeltaTime = scaledDeltaTime;
            OnUpdate?.Invoke(scaledDeltaTime);
        }


        ~ScalableUpdateTime() => OnDispose();

        public void Dispose()
        {
            OnDispose();
            GC.SuppressFinalize(this);
        }

        private void OnDispose()
        {
            if (disposed)
                return;

            updateNotifier.OnUpdate -= OnUpdateNotified;
            disposed = true;
        }
    }
}
