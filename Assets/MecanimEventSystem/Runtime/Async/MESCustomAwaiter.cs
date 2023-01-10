using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using zFrame.Event;
public enum Event
{
    OnStart,
    OnEnd
}

public static class MESCustomAwaiter
{
    public static AnimationAwaiter SetBoolAsync(this EventState state, string paramName, bool value, Event type = Event.OnEnd)
    {
        var awaiter = new AnimationAwaiter();
        switch (type)
        {
            case Event.OnStart:
                state.OnStart(v=>awaiter.Complete());
                break;
            case Event.OnEnd:
                state.OnCompleted(v=>awaiter.Complete());
                break;
        }
        state.SetBool(paramName, value);
        return awaiter;
    }
}

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
    public object GetResult() => null;
    public void Complete()
    {
        _isDone = true;
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


