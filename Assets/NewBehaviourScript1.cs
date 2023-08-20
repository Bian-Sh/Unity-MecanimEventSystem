using System;
using UnityEngine;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(NewBEditor))]
class NewBEditor : Editor
{
    NewBehaviourScript1 OBJ;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (OBJ == null)
        {
            OBJ = target as NewBehaviourScript1;
        }
        // 如何通过反射等手段

    }
}


[CustomPropertyDrawer(typeof(AnimatorParams))]
public class AnimatorParamsDrawer : PropertyDrawer
{
    AnimatorParams @params;
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        var stateSP = property.FindPropertyRelative(nameof(AnimatorParams.state));
        var paramSP = property.FindPropertyRelative(nameof(AnimatorParams.param));
        var valueSP = property.FindPropertyRelative(nameof(AnimatorParams.value));
        EditorGUI.PropertyField(amountRect, stateSP, GUIContent.none);
        EditorGUI.PropertyField(unitRect, paramSP, GUIContent.none);
        //todo: 根据 params name （AnimatorParams.param）去 Animator 查找是否存在且属于 什么参数类型（AnimatorControllerParameterType）
        // 一下是demo
        var paramsType = AnimatorControllerParameterType.Bool;
        if (paramSP.stringValue == "boolvalue")
        {
            paramsType = AnimatorControllerParameterType.Bool;
        }
        else if (paramSP.stringValue == "intvalue")
        {
            paramsType = AnimatorControllerParameterType.Int;
        }

        switch (paramsType)
        {
            case AnimatorControllerParameterType.Bool:
                @params.value = false;
                //valueSP.boolValue = false;
                break;
            case AnimatorControllerParameterType.Int:
                valueSP.intValue = 111;
                break;
            default:
                Debug.Log($"{nameof(AnimatorParamsDrawer)}: todo ....");
                break;
        }
        EditorGUI.PropertyField(nameRect, valueSP, GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif

public class NewBehaviourScript1 : MonoBehaviour
{
    [SerializeField]
    AnimatorParams animp;
    [SerializeField]
    Animator animator;
    void Start()
    {
        animator.SetTriggerAsync(animp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

[Serializable]
public class AnimatorParams
{
    public string state; //Base Layer.Expand
    public string param; //
    public object value; // 1.0f ,true ,false ,1
    public AnimatorParams(string state, string param, object value)
    {
        this.state = state;
        this.param = param;
        this.value = value;
    }
    public override string ToString()
    {
        var info = @$"state = {state}
param = {param}
vale = {value}";
        return info;
    }
}

public static class AnimatorEx
{
    public static Task<AnimationEvent> SetTriggerAsync(this Animator animator, AnimatorParams @params)
    {
        Debug.Log($"{nameof(AnimatorEx)}: params = {@params}");
        return default;
    }
}