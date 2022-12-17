using System;
using System.Runtime.CompilerServices;
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
                state.OnStart(v => awaiter.Complete());
                break;
            case Event.OnEnd:
                state.OnCompleted(v => awaiter.Complete());
                break;
        }
        state.SetBool(paramName, value);
        return awaiter;
    }
}

public class AnimationAwaiter : INotifyCompletion
{
    private bool _isDone;
    Action _continuation;

    public bool IsCompleted => _isDone;
    public void OnCompleted(Action continuation)
    {
        _isDone = true;
        _continuation = continuation;
    }

    public AnimationAwaiter GetAwaiter() => this;
    public object GetResult() => null;
    public void Complete()
    {
        _isDone = true;
        _continuation?.Invoke();
    }
}


