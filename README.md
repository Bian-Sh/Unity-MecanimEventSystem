# Unity-MecanimEventSystem
# Animator 事件回调系统

# 简述
这是一个非常实用的，链式编程风格的 Animator 事件系统。

通过 方法扩展 的方式实现了对 Animator 的功能扩展

现在你能直接使用 Animator.SetTarget(clipname,frame) 指定在哪一个动画片段的哪一帧上插入事件。

解决了必须手动插入AnimationEvent的痛点，同时链式+ Lambda 使得逻辑更集中更优雅，也方便阅读和理解。

插入的事件分三个：

|API|Description|
-|-
OnStart| 在动画片段的第一帧执行的回调
OnProcess|在动画片段用户指定帧执行的回调
OnCompleted|在动画片段结束帧执行的回调

# 使用实例

> * 在中间帧
```
 animator.SetTarget("Left", 55)   //1. 拿到 Animator 引用
    .OnProcess((v) =>             //2. 拆入事件到 55帧 
    {
        string clipname = v.animatorClipInfo.clip.name;       //3.拿到动画片段信息--------片段名称 
        if (v.animatorStateInfo.IsName("Base Layer.Rotate")   //4.拿到动画状态信息--------状态名称 
        {
            Debug.Log("结束时Base Layer：" + clipname + ":" + v.time * v.animatorClipInfo.clip.frameRate); //5. 基于上面的信息做层别以及其他逻辑
        }
        if (v.animatorStateInfo.IsName("New Layer.Rotate1212"))//演示其它层的事件接受
        {
            Debug.Log("结束时New Layer：" + clipname + ":" + v.time * v.animatorClipInfo.clip.frameRate);
        }
    })
    .SetParms("ddsf", objectParm: gameObject) //6. 演示在事件中放置参数，但闭包优势，参数均在上下文，所以可以不设置参数
    .SetTrigger("Left"); // 7. 适配器--适配了Animator API 可以链式操控动画机
```
> * 在开始和结束帧
```
 animator.SetTarget("Rotate")
         .OnStart(v=>
         {
            //Do SomeThing
         })
         .OnCompleted((v) =>
         {
            //Do something when finished            
          });
```
# 注意事项
因为对 AnimationClip 绑定事件的时候必须全面刷新 AnimationClip 数据，Animator也会刷新，所以使用本扩展绑定事件的动作必须放在所有的Animator操作之前。

As I use ``Animator.Rebind()`` to binding event into a AnimationClip ,the Animator will refresh then，so any operarion about Animator must behind MecanimEventSystem's API which register events.


# 动画演示

这个仓库有 Example 没录制 gif


# 我的简书

[Unity 3D 打造自己的Mecanim Callback System - 简书](https://www.jianshu.com/p/d68b6813c74f)







