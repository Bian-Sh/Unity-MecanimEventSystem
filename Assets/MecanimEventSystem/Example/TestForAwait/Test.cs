using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using zFrame.Event;

public class Test : MonoBehaviour
{
    public Animator animator;
    public Button button;

    public Text text, text2;
    public int delay = 5;
    float countcached = 0;
    private bool isCounting = false;

    EventState callbackExp, callbackClps;


    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
        text.text = "expand";
    }

    private void Update()
    {
        if (isCounting)
        {
            countcached += Time.deltaTime;
            text2.text = countcached.ToString("f0");
        }
    }

    private async void OnButtonClicked()
    {
        // 等待折叠
        {
            button.interactable = false;
            Debug.Log($"{nameof(Test)}:  Default is  expand , Now start collapse！");
            var r = await animator.SetBoolAsync("Expand","Expand", false); // todo: 必须细分到 layer.state 这种级别，否则误触会很伤 
            Debug.Log($"{nameof(Test)}:  collapse Completed , clip name = {r.animatorClipInfo.clip.name}！");
            text.text = "collapsed";
        }

        //做一个延迟
        {
            Debug.Log($"{nameof(Test)}:  wait for {delay} second！");
            isCounting = true;
            await Task.Delay(delay * 1000);
            isCounting = false;
            countcached = 0;
            text2.text = string.Empty;
            Debug.Log($"{nameof(Test)}:  waiting finish！");
        }

        // 等待展开
        {
            Debug.Log($"{nameof(Test)}:  Now is collapse , expanding ！");
            await animator.SetBoolAsync("Expand", "Expand", true);
            text.text = "expand";
            Debug.Log($"{nameof(Test)}: expand Completed!");
            button.interactable = true;
        }
    }
}
