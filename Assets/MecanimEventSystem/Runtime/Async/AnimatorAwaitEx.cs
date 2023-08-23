using UnityEngine;
using zFrame.Event;

public static class AnimatorAwaitEx 
{
    public static AnimationAwaiter SetBoolAsync(this Animator animator,string clipName, string paramName, bool value)
    {
        var awaiter = new AnimationAwaiter();
        var state = animator.SetTarget(clipName);
        state.OnCompleted(awaiter.SetResult);
        state.SetBool(paramName, value);
        return awaiter;
    }

    public static AnimationAwaiter SetTriggerAsync(this Animator animator, string clipName, string paramName)
    {
        var awaiter = new AnimationAwaiter();
        var state = animator.SetTarget(clipName);
        state.OnCompleted(awaiter.SetResult);
        state.SetTrigger(paramName);
        return awaiter;
    }

    public static AnimationAwaiter SetFloatAsync(this Animator animator, string clipName, string paramName, float value)
    {
        var awaiter = new AnimationAwaiter();
        var state = animator.SetTarget(clipName);
        state.OnCompleted(awaiter.SetResult);
        state.SetFloat(paramName, value);
        return awaiter;
    }
}


