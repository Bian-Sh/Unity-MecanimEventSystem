using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using zFrame.Event;

public class Test : MonoBehaviour
{
    public Animator animator;
    public Button button;

    public Text text;

    EventState callbackExp, callbackClps;


    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
        text.text = "expand";

        // 运行时为动画片段绑定事件, 提前绑定以避免Animator运行过程中被重新绑定
        // 发现 ReBind 会导致第二个 settarget 延迟到最后，此为异常
        // 解决方案是编辑器下提前安插事件
       callbackExp = animator.SetTarget("Expand");
        callbackClps = animator.SetTarget("Collapse");
    }

    private async void OnButtonClicked()
    {
        button.interactable = false;
        Debug.Log($"{nameof(Test)}:  Default is  expand , Now start collapse！");

        await callbackClps.SetBoolAsync("Expand",false);
        
        Debug.Log($"{nameof(Test)}:  collapse Completed ！");
        text.text = "collapsed";
        
        Debug.Log($"{nameof(Test)}:  wait for 5 second！");
        await Task.Delay(5000);
        Debug.Log($"{nameof(Test)}:  waiting finish！");

        Debug.Log($"{nameof(Test)}:  Now is collapse , expanding ！");
        await callbackExp.SetBoolAsync("Expand", true); 
        text.text = "expand";
        Debug.Log($"{nameof(Test)}: expand Completed!");
        button.interactable = true;
    }
}
