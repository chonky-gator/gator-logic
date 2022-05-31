using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
#if PACKAGE_UNITASK
using Cysharp.Threading.Tasks;
#endif

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
        
#if PACKAGE_UNITASK
        public static UniTask StartCoroutineAsTask(this MonoBehaviour monoBehaviour, IEnumerator routine)
        {
            return routine.ToUniTask(monoBehaviour);
        }
#endif
    }
}
