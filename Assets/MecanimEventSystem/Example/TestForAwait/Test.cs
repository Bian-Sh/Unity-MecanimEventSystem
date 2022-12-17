using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using zFrame.Event;

public class Test : MonoBehaviour
{
    public Animator animator;
    public Button button;
    
    
    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);        
    }

    private async void OnButtonClicked()
    {
        // 运行时为动画片段绑定事件, 提前绑定以避免Animator运行过程中被重新绑定
        // 发现 ReBind 会导致第二个 settarget 延迟到最后，此为异常
        // 解决方案是编辑器下提前安插事件
        var callbackExp = animator.SetTarget("Expand");
        var callbackClps = animator.SetTarget("Collapse");
            
        button.interactable = false;
        
        Debug.Log($"{nameof(Test)}:  Default is  collapse, Now start expand！");

        await callbackExp.SetBoolAsync("Expand",true);
        Debug.Log($"{nameof(Test)}:  expand Completed ！");
        
        Debug.Log($"{nameof(Test)}:  waiting 2 second！");
        await Task.Delay(2000);
        Debug.Log($"{nameof(Test)}:  waiting finish！");

        Debug.Log($"{nameof(Test)}:  Now is expand  , now start collapse！");
        await callbackClps.SetBoolAsync("Expand", false); 
        Debug.Log($"{nameof(Test)}: collapse Completed!");
        button.interactable = true;
    }
}
