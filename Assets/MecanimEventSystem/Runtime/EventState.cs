using System;
using UnityEngine;
using static zFrame.Event.EventHandler;

namespace zFrame.Event
{
    /// <summary>Mecanim事件系统事件配置类_for start+completed callback </summary>
    public class EventState
    {
        protected AnimationEvent _ClipEvent;
        protected int _keyFrame;
        protected EventInfo a_Event;
        protected Animator _animator;
        /// <summary>
        /// 为Clip添加Onstart回调事件
        /// </summary>
        /// <param name="onStart">回调</param>
        /// <returns>参数配置器</returns>
        public EventState OnStart(Action<AnimationEvent> onStart)
        {
            if (a_Event == null) return null;
            ConfigEvent(0, onStart);
            return this;
        }
        /// <summary>
        /// 为Clip添加OnCompleted回调事件
        /// </summary>
        /// <param name="OnCompleted">回调</param>
        /// <returns>参数配置器</returns>
        public EventState OnCompleted(Action<AnimationEvent> onCompleted)
        {
            if (a_Event == null) return null;
            ConfigEvent(a_Event.totalFrames, onCompleted);
            return this;
        }

        public EventState OnProcess(Action<AnimationEvent> onProcess)
        {
            if (a_Event == null) return null;
            ConfigEvent(_keyFrame, onProcess);
            return this;
        }

        public EventState(EventInfo eventInfo, int frame = -1)
        {
            //如果用户不指定帧则默认是最后一帧
            _keyFrame = frame == -1 ? eventInfo.totalFrames : frame; 
            a_Event = eventInfo;
            _animator = eventInfo.animator;
        }

        /// <summary>
        /// 为指定帧加入回调链
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="action"></param>
        protected void ConfigEvent(int frame, Action<AnimationEvent> action)
        {
            if (null == action) return;
            _keyFrame = frame;
            if (!a_Event.frameCallBackPairs.TryGetValue(_keyFrame, out var actions))
            {
                actions = new();
                a_Event.frameCallBackPairs[_keyFrame] = actions;
            }
            if (!actions.Contains(action))
            {
                actions.Add(action);
                GenerateAnimationEvent(a_Event, _keyFrame);
            }
            else
            {
                Debug.LogWarning($"AnimatorEventSystem[一般]：指定AnimationClip【{a_Event.animationClip.name}】已经订阅了该事件【{action.Method.Name}】！\n 建议：请勿频繁订阅！");
            }
        }

        #region Adapter For Animator
        /// <summary>
        /// 设置动画机bool参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public Animator SetBool(string name, bool value)
        {
            _animator.SetBool(name, value);
            return _animator;
        }
        /// <summary>
        /// 设置动画机bool参数
        /// </summary>
        /// <param name="name">参数id</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public Animator SetBool(int id, bool value)
        {
            _animator.SetBool(id, value);
            return _animator;
        }
        /// <summary>
        /// 设置动画机float参数
        /// </summary>
        /// <param name="name">参数id</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public Animator SetFloat(int id, float value)
        {
            _animator.SetFloat(id, value);
            return _animator;
        }
        /// <summary>
        /// 设置动画机float参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public Animator SetFloat(string name, float value)
        {
            _animator.SetFloat(name, value);
            return _animator;
        }
        /// <summary>
        ///  设置动画机float参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="dampTime"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public Animator SetFloat(string name, float value, float dampTime, float deltaTime)
        {
            _animator.SetFloat(name, value, dampTime, deltaTime);
            return _animator;
        }
        /// <summary>
        /// 设置动画机float参数
        /// </summary>
        /// <param name="name">参数id</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public Animator SetFloat(int id, float value, float dampTime, float deltaTime)
        {
            _animator.SetFloat(id, value, dampTime, deltaTime);
            return _animator;
        }
        /// <summary>
        /// 设置动画机trigger参数
        /// </summary>
        /// <param name="name">参数id</param>
        /// <returns></returns>
        public Animator SetTrigger(int id)
        {
            _animator.SetTrigger(id);
            return _animator;
        }
        /// <summary>
        /// 设置动画机trigger参数
        /// </summary>
        /// <param name="name">参数name</param>
        /// <returns></returns>
        public Animator SetTrigger(string name)
        {
            _animator.SetTrigger(name);
            return _animator;
        }
        #endregion
    }
}
