using UnityEngine;
namespace zFrame.Event.Example
{
    public class EventListener : MonoBehaviour
    {
        Animator animator;
        private void Start()
        {
            animator = GameObject.Find("Cube").GetComponent<Animator>();
            animator.SetTarget("Left", 55).OnProcess((v) =>
            {
                string clipname = v.animatorClipInfo.clip.name;  //无差别触发下面的逻辑，因为没有对（Layer）层进行层别，如果这个AnimationClip在这个动画机中被多次复用，请务必层别层信息（如下面示例）
                Debug.Log("55帧到了：" + clipname + ":" + v.time * v.animatorClipInfo.clip.frameRate + "搞事情！");
            });
        }
        private void OnGUI()
        {
            if (GUILayout.Button("Rotate动态添加回调 （oneshot）"))
            {
                animator.SetTarget("Rotate").OnCompleted((v) =>
                {
                    string clipname = v.animatorClipInfo.clip.name;
                    if (v.animatorStateInfo.IsName("Base Layer.Rotate"))//演示Base Layer的事件接受 ---因为AnimationClip在这个动画机中被多次复用，层别Layer信息是非常必要的（区别于上面的示例）
                    {
                        Debug.Log("结束时Base Layer：" + clipname + ":" + v.time * v.animatorClipInfo.clip.frameRate);
                    }

                    if (v.animatorStateInfo.IsName("New Layer.Rotate1212"))//演示其它层的事件接受
                    {
                        Debug.Log("结束时New Layer：" + clipname + ":" + v.time * v.animatorClipInfo.clip.frameRate);
                    }
                });
            }

            if (GUILayout.Button("触发baseLayer.Left （oneshot）"))
            {
                animator.SetTrigger("Left");
            }

        }
    }
}
