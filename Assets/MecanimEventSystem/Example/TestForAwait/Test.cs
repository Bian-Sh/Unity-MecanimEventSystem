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
        // ����ʱΪ����Ƭ�ΰ��¼�, ��ǰ���Ա���Animator���й����б����°�
        // ���� ReBind �ᵼ�µڶ��� settarget �ӳٵ���󣬴�Ϊ�쳣
        // ��������Ǳ༭������ǰ�����¼�
        var callbackExp = animator.SetTarget("Expand");
        var callbackClps = animator.SetTarget("Collapse");
            
        button.interactable = false;
        
        Debug.Log($"{nameof(Test)}:  Default is  collapse, Now start expand��");

        await callbackExp.SetBoolAsync("Expand",true);
        Debug.Log($"{nameof(Test)}:  expand Completed ��");
        
        Debug.Log($"{nameof(Test)}:  waiting 2 second��");
        await Task.Delay(2000);
        Debug.Log($"{nameof(Test)}:  waiting finish��");

        Debug.Log($"{nameof(Test)}:  Now is expand  , now start collapse��");
        await callbackClps.SetBoolAsync("Expand", false); 
        Debug.Log($"{nameof(Test)}: collapse Completed!");
        button.interactable = true;
    }
}
