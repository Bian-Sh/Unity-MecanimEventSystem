using System.Numerics;
using UnityEditor;
using UnityEngine;
using static zFrame.Event.EventHandler;

namespace zFrame.Event
{
    //代码分析，编辑器下就把事件绑定好
    //Please use Editor.AnimationUtility to add persistent animation events to an animation clip
    //UnityEngine.AnimationClip:AddEvent(UnityEngine.AnimationEvent)
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
            foreach (var item in actions)
            {
                item?.Invoke(ae);
            }
        }
    }
}
