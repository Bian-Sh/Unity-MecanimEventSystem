using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum LogLevel
{
    None = 1,
    Error = 2,
    Warning = 4,
    Info = 8,
    All = Error | Warning | Info
}

namespace zFrame.Event
{
    public static class EventHandler
    {
        /// <summary>动画机及其事件信息Pairs</summary>
        private static List<EventInfo> eventContainer = new List<EventInfo>();
        public static LogLevel LogLevels = LogLevel.All;

        private const string func = "AnimatorEventCallBack";

        /// <summary>
        /// 为事件基础信息进行缓存
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="clipName">动画片段名称</param>
        /// <param name="frame">指定帧</param>
        public static EventInfo GetAnimationInfo(Animator animator, string clipName, bool createIfNotFound = true)
        {
            AnimationClip clip = GetAnimationClip(animator, clipName);
            if (clip)
            {
                return GetEventInfo(animator, clip, createIfNotFound);  //获取指定事件信息类
            }
            else
            {
                throw new Exception($"{clipName} 不存在于 {animator}");
            }
        }

        /// <summary>
        /// 为指定动画机片段插入回调方法 
        /// </summary>
        /// <param name="eventInfo">回调信息类</param>
        /// <param name="frame">指定帧</param>
        public static void GenerateAnimationEvent(EventInfo eventInfo, int frame)
        {
            if (frame < 0 || frame > eventInfo.totalFrames)
            {
                if (LogLevels.HasFlag(LogLevel.Error))
                {
                    Debug.LogError($"AnimatorEventSystem[紧急]：【{eventInfo.animator.name}】所在的动画机【{eventInfo.animationClip.name}】片段帧数设置错误【{frame}】！");
                }
                return;
            }
            float _time = frame / eventInfo.animationClip.frameRate;
            AnimationEvent[] events = eventInfo.animationClip.events;
            AnimationEvent varEvent = Array.Find(events, v => Mathf.Approximately(v.time, _time));
            if (null != varEvent)
            {
                if (varEvent.functionName == func) return;
                if (LogLevels.HasFlag(LogLevel.Info))
                {
                    Debug.Log($"AnimatorEventSystem[一般]：【{eventInfo.animator.name}】所在的动画机【{eventInfo.animationClip.name}】片段【{frame}】帧已存在回调方法【{varEvent.functionName}】，将自动覆盖！");
                }
            }
            varEvent = new()
            {
                functionName = func, //指定事件的函数名称
                time = _time,  //对应动画指定帧处触发
                messageOptions = SendMessageOptions.DontRequireReceiver, //回调未找到不提示
            };
            eventInfo.animationClip.AddEvent(varEvent); //绑定事件
            eventInfo.animator.Rebind(); //重新绑定动画器的所有动画的属性和网格数据。
            if (LogLevels.HasFlag(LogLevel.Info))
            {
                Debug.Log($"{nameof(EventHandler)}:完成 AnimationEvent 添加, 建议优先在编辑器下就把事件安插OK以避免动画机的重新绑定，see more ↓ \nClip Name = {eventInfo.animationClip.name} , frame = {frame} , time = {_time} ,Function  Name = {func}");
            }
        }

        /// <summary>数据重置，用于总管理类清理数据用</summary>
        public static void Clear()
        {
            foreach (var item in eventContainer)
            {
                item.Clear();
            }
            eventContainer = new List<EventInfo>();
        }

        #region Helper Function
        /// <summary>
        /// 获得指定的事件信息类
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="clip">动画片段</param>
        /// <returns>事件信息类</returns>
        private static EventInfo GetEventInfo(Animator animator, AnimationClip clip, bool createIfNotFound = true)
        {
            EventInfo a_EventInfo = eventContainer.Find(v => v.animator == animator && v.animationClip == clip);
            if (null == a_EventInfo && createIfNotFound)
            {
                a_EventInfo = new EventInfo(animator, clip);
                eventContainer.Add(a_EventInfo);
            }
            return a_EventInfo;
        }

        /// <summary>
        /// 根据动画片段名称从指定动画机获得动画片段
        /// </summary>
        /// <param name="animator">动画机</param>
        /// <param name="name">动画片段名称</param>
        /// <returns></returns>
        public static AnimationClip GetAnimationClip(Animator animator, string name)
        {
            #region 异常提示
            if (null == animator)
            {
                if (LogLevels.HasFlag(LogLevel.Error))
                {
                    Debug.LogError("AnimatorEventSystem[紧急]：指定Animator不得为空！");
                }
                return null;
            }
            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
            if (null == runtimeAnimatorController)
            {
                if (LogLevels.HasFlag(LogLevel.Error))
                {
                    Debug.LogError("AnimatorEventSystem[紧急]：指定【" + animator.name + "】Animator未挂载Controller！");
                }
                return null;
            }
            AnimationClip[] clips = runtimeAnimatorController.animationClips;
            AnimationClip[] varclip = Array.FindAll(clips, v => v.name == name);
            if (null == varclip || varclip.Length == 0)
            {
                throw new InvalidOperationException("AnimatorEventSystem[紧急]：指定【" + animator.name + "】Animator不存在名为【" + name + "】的动画片段！");
            }
            if (varclip.Length >= 2)
            {
                if (LogLevels.HasFlag(LogLevel.Warning))
                {
                    Debug.LogWarning($"AnimatorEventSystem[一般]：指定【{animator.name}】Animator存在【{varclip.Length}】个名为【{name}】的动画片段！\n 建议：若非复用导致的重名，请务必修正！否则，事件将绑定在找的第一个Clip上。");
                }
            }
            #endregion
            return varclip[0];
        }
        /// <summary>
        /// 根据给定信息获得委托
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clip"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static List<Action<AnimationEvent>> GetAction(Animator animator, AnimationClip clip, int frame)
        {
            List<Action<AnimationEvent>> actions = default;
            EventInfo a_EventInfo = eventContainer.Find(v => v.animator == animator && v.animationClip == clip);
            if (null != a_EventInfo)
            {
                if (!a_EventInfo.frameCallBackPairs.TryGetValue(frame, out actions))
                {
                    actions = new();
                    if (LogLevels.HasFlag(LogLevel.Warning))
                    {
                        Debug.LogWarning($"{nameof(EventHandler)}: Key [frame = {frame}] dose not exsit! \n {animator.name} - {clip.name}");
                    }
                }
            }
            return actions;
        }
        #endregion 
    }
}
