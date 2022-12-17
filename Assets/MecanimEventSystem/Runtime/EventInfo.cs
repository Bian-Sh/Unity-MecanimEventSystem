using System;
using System.Collections.Generic;
using UnityEngine;

namespace zFrame.Event
{
    /// <summary>
    /// 事件订阅信息信息存储类
    /// </summary>
    public class EventInfo
    {
        /// <summary>订阅事件所在的动画机</summary>
        public Animator animator;
        /// <summary>动画机指定状态的动画片段</summary>
        public AnimationClip animationClip;
        /// <summary>动画片段的总帧数</summary>
        public int totalFrames;
        /// <summary>帧以及对应的回调链</summary>
        public Dictionary<int, List<Action<AnimationEvent>>> frameCallBackPairs;
        public EventInfo(Animator anim, AnimationClip clip)
        {
            frameCallBackPairs = new();
            animator = anim;
            animationClip = clip;
            //经验表明需要 向下取整 以获取当前的帧数
            totalFrames = Mathf.FloorToInt(animationClip.frameRate * animationClip.length);
        }

        /// <summary>清除数据</summary>
        public void Clear()
        {
            animationClip.events = default;
            frameCallBackPairs = new();
            animationClip = null;
            animator = null;
        }
    }
}
