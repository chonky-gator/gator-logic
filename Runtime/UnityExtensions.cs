using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GatOR.Logic
{
    public static class UnityExtensions
    {
        public static T Or<T>(this T target, T ifNullOrDestroyed) where T : Object =>
            target ? target : ifNullOrDestroyed;

        public static T Or<T>(this T target, Func<T> ifNullOrDestroyed) where T : Object =>
            target ? target : ifNullOrDestroyed();

        public static T OrThrowNullArgument<T>(this T target, string argName) where T : Object =>
            target ? target : throw new ArgumentNullException(argName);

        public static Coroutine StartCoroutine(this MonoBehaviour monoBehaviour,
            Func<IEnumerator> enumeratorFunction) =>
            monoBehaviour.StartCoroutine(enumeratorFunction());
    }
}
