using System;
using UnityEngine;
namespace zFrame.Event
{
    /// <summary>Mecanim事件系统事件配置类_for start+completed callback </summary>
    public class EventConfig_A : BaseEventConfig
    {
        public EventConfig_A(EventInfo eventInfo, int frame = -1) : base(eventInfo, frame) { }
        /// <summary>
        /// 为Clip添加Onstart回调事件
        /// </summary>
        /// <param name="onStart">回调</param>
        /// <returns>参数配置器</returns>
        public EventConfig_A OnStart(Action<AnimationEvent> onStart)
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
        public  EventConfig_A OnCompleted(Action<AnimationEvent> onCompleted)
        {
            if (a_Event == null) return null;
            ConfigEvent(a_Event.totalFrames,onCompleted);
            return this;
        }
    }
    /// <summary>Mecanim事件系统事件配置类_For Process callback </summary>
    public class EventConfig_B : BaseEventConfig
    {
        public EventConfig_B(EventInfo eventInfo, int frame) : base(eventInfo, frame) { }
        public EventConfig_B OnProcess(Action<AnimationEvent> onProcess)
        {
            if (a_Event == null) return null;
            ConfigEvent(_keyFrame, onProcess);
            return this;
        }
    }
}
