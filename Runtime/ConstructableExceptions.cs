

namespace GatOR.Logic
{

    [System.Serializable]
    public class AlreadyConstructedException : System.Exception
    {
        public AlreadyConstructedException() { }
        public AlreadyConstructedException(System.Exception inner) : base(null, inner) { }
        protected AlreadyConstructedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class NotConstructedException : System.Exception
    {
        public NotConstructedException() { }
        public NotConstructedException(System.Exception inner) : base(null, inner) { }
        protected NotConstructedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
