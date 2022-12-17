using UnityEngine;
using static zFrame.Event.EventHandler;

namespace zFrame.Event
{
    public static class EventExtension
    {
        /// <summary>
        /// 指定需要绑定回调的AnimationClip
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="clipName">动画片段</param>
        /// <returns>事件配置器</returns>
        public static EventState SetTarget(this Animator animator, string clipName)
        {
            EventInfo a_EventInfo = GenerAnimationInfo(animator, clipName);
            animator.EnsureComponent<CallbackListener>();
            //获得需要处理的动画片段
            return new EventState(a_EventInfo);
        }

        /// <summary>
        /// 指定需要绑定回调的AnimationClip
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="clipName">动画片段</param>
        /// <returns>事件配置器</returns>
        public static EventState GetEventConfig(this Animator animator, string clipName)
        {
            EventInfo a_EventInfo = GenerAnimationInfo(animator, clipName);
            animator.EnsureComponent<CallbackListener>();
            //获得需要处理的动画片段
            return new EventState(a_EventInfo);
        }
        /// <summary>
        /// 指定需要绑定回调的AnimationClip
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="clipName">动画片段</param>
        /// <param name="frame">动画片段指定帧</param>
        /// <returns>事件配置器</returns>
        public static EventState SetTarget(this Animator animator, string clipName, int frame)
        {
            EventInfo a_EventInfo = GenerAnimationInfo(animator, clipName);
            animator.EnsureComponent<CallbackListener>();
            //获得需要处理的动画片段
            return new EventState(a_EventInfo, frame);
        }


        private static T EnsureComponent<T>(this Component target) where T : Component
        {
            T component = target.GetComponent<T>();
            if (!component)
            {
                component = target.gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}
