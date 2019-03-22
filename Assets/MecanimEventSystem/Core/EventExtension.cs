using UnityEngine;

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
        public static EventConfig_A SetTarget(this Animator animator, string clipName)
        {
            EventInfo a_EventInfo = EventHandler.Instance.GenerAnimationInfo(animator, clipName);
            if (null != a_EventInfo)
            {
                if (null == animator.GetComponent<CallbackListener>())
                {
                    animator.gameObject.AddComponent<CallbackListener>();
                }
            }
            //获得需要处理的动画片段
            return new EventConfig_A(a_EventInfo);
        }
        /// <summary>
        /// 指定需要绑定回调的AnimationClip
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="clipName">动画片段</param>
        /// <param name="frame">动画片段指定帧</param>
        /// <returns>事件配置器</returns>
        public static EventConfig_B SetTarget(this Animator animator, string clipName, int frame)
        {
            EventInfo a_EventInfo = EventHandler.Instance.GenerAnimationInfo(animator, clipName);
            if (null != a_EventInfo)
            {
                if (null == animator.GetComponent<CallbackListener>())
                {
                    animator.gameObject.AddComponent<CallbackListener>();
                }
            }
            //获得需要处理的动画片段
            return new EventConfig_B(a_EventInfo, frame);
        }
    }
}
