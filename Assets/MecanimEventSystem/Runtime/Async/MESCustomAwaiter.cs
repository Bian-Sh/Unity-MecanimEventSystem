using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class AnimationAwaiter : INotifyCompletion
{
    static SynchronizationContext synchronizContent;
    static int id;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void CaptureSynchronizationContext()
    {
        id = Thread.CurrentThread.ManagedThreadId;
        synchronizContent = SynchronizationContext.Current;
    }

    private bool _isDone;
    Action _continuation;
    public bool IsCompleted => _isDone;
    public void OnCompleted(Action continuation)
    {
        _continuation = continuation;
    }

    public AnimationAwaiter GetAwaiter() => this;
    public AnimationEvent GetResult() => animationEvent;
    AnimationEvent animationEvent;
    public void SetResult(AnimationEvent ae)
    {
        _isDone = true;
        animationEvent = ae;
        if (Thread.CurrentThread.ManagedThreadId == id)
        {
            _continuation?.Invoke();
        }
        else
        {
            synchronizContent.Post(v => _continuation?.Invoke(), null);
        }
    }
}


