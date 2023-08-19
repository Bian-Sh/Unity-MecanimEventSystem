using System;
using System.Collections.Generic;
using UnityEngine;
using static zFrame.Event.EventHandler;

namespace zFrame.Event
{
    [RequireComponent(typeof(Animator))]
    public class CallbackListener : MonoBehaviour
    {
        Animator animator;
        void Awake() => animator = GetComponent<Animator>();
        /// <summary>通用事件回调</summary>
        /// <param name="ae">事件传递的参数信息</param>
        private void AnimatorEventCallBack(AnimationEvent ae)
        {
            AnimationClip clip = ae.animatorClipInfo.clip;//动画片段名称
            int currentFrame = Mathf.FloorToInt(ae.animatorClipInfo.clip.frameRate * ae.time);  //动画片段当前帧 向下取整
            var actions = GetAction(animator, clip, currentFrame);
            var temp = new List<Action<AnimationEvent>>(actions);
            for (int i = 0; i < temp.Count; i++)
            {
                var action = temp[i];
                if (action != null)
                {
                    action.Invoke(ae);
                    // the callback  which comes from AnimationAwaiter should be oneshot event.
                    if (action.Method.DeclaringType.Name == nameof(AnimationAwaiter))
                    {
                        actions.Remove(action);
                    }
                }
            }
        }
    }
}
