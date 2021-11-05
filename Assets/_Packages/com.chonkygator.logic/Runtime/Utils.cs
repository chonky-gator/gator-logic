using System;
using Object = UnityEngine.Object;

namespace GatOR.Logic
{
    public static class Utils
    {
        public static T Or<T>(this T target, T ifNullOrDestroyed) where T : Object =>
            target ? target : ifNullOrDestroyed;

        public static T Or<T>(this T target, Func<T> ifNullOrDestroyed) where T : Object =>
            target ? target : ifNullOrDestroyed();
    }
}
