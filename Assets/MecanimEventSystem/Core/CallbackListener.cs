using System;
using UnityEngine;

namespace zFrame.Event
{
    public class CallbackListener : MonoBehaviour
    {
        Animator animator;
        EventHandler eventHandler;
        void Awake()
        {
            animator = GetComponent<Animator>();
            eventHandler = EventHandler.Instance; 
        }
        /// <summary>通用事件回调</summary>
        /// <param name="ae">事件传递的参数信息</param>
        private void AnimatorEventCallBack(AnimationEvent ae)
        {
            AnimationClip clip = ae.animatorClipInfo.clip;//动画片段名称
            int currentFrame = Mathf.FloorToInt(ae.animatorClipInfo.clip.frameRate* ae.time);  //动画片段当前帧 向下取整
            eventHandler.GetAction(animator, clip, currentFrame)?.Invoke(ae);
        }
    }
}
