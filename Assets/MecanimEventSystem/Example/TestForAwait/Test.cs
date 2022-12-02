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
        // 运行时为动画片段绑定事件
        var callbackExp =  animator.SetTarget("Expand");
        var callbackClps = animator.SetTarget("Collapse");

        button.interactable = false;
        Debug.Log($"{nameof(Test)}:  Expand Start！");
        await callbackExp.SetBoolAsync("Expand",true);
        Debug.Log($"{nameof(Test)}:  Expand Completed ！");
        
      //  await Task.Delay(2000);

        Debug.Log($"{nameof(Test)}:  Collapse Start！");
        await callbackClps.SetBoolAsync("Expand", false); 
        Debug.Log($"{nameof(Test)}: Collapse Completed!");
        button.interactable = true;
    }
}
