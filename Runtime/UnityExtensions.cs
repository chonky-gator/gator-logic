using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

#if PACKAGE_UNITASK
using System.Threading;
using Cysharp.Threading.Tasks;
#endif

namespace GatOR.Logic
{
    public static class UnityExtensions
    {
        public static T Or<T>(this T target, T ifNullOrDestroyed) where T : Object
        {
            return target ? target : ifNullOrDestroyed;
        }

        public static T Or<T>(this T target, Func<T> ifNullOrDestroyed) where T : Object
        {
            return target ? target : ifNullOrDestroyed();
        }

        public static T OrThrowNullArgument<T>(this T target, string argName) where T : Object
        {
            return target ? target : throw new ArgumentNullException(argName);
        }

        public static Coroutine StartCoroutine(this MonoBehaviour monoBehaviour,
            Func<IEnumerator> enumeratorFunction) =>
            monoBehaviour.StartCoroutine(enumeratorFunction());

        public static void LogException(this Object context, Exception exception)
        {
            Debug.LogException(exception, context);
        }

#if PACKAGE_UNITASK
        public static UniTask StartCoroutineAsTask(this MonoBehaviour monoBehaviour, Func<IEnumerator> routine,
            CancellationToken cancellationToken = default)
        {
            return monoBehaviour.StartCoroutineAsTask(routine(), cancellationToken);
        }

        public static UniTask StartCoroutineAsTask(this MonoBehaviour monoBehaviour, IEnumerator routine,
            CancellationToken cancellationToken = default)
        {
            var source = AutoResetUniTaskCompletionSource.Create();
            var coroutine = monoBehaviour.StartCoroutine(Routine());
            cancellationToken.Register(() => monoBehaviour.StopCoroutine(coroutine));
            return source.Task;

            IEnumerator Routine()
            {
                yield return routine;
                source.TrySetResult();
            }
        }

        public static void Forget(this UniTask task, Object context) => task.Forget(context.LogException);
        
        public static void Forget<T>(this UniTask<T> task, Object context) => task.Forget(context.LogException);
#endif
    }
}
