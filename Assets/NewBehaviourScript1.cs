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
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative(nameof(AnimatorParams.state)), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative(nameof(AnimatorParams.param)), GUIContent.none);
        // 根据 AnimatorParams.type 绘制 AnimatorParams.value
        var type = (AnimatorControllerParameterType)property.FindPropertyRelative(nameof(AnimatorParams.type)).enumValueIndex;
        switch (type)
        {
            case AnimatorControllerParameterType.Bool:
                // 将 object 类型的AnimatorParams.value  绘制成复选框
                
                break;
            case AnimatorControllerParameterType.Int:
                var sp = property.FindPropertyRelative(nameof(AnimatorParams.value));
                var value = property.intValue;
                // draw int field
                EditorGUI.PropertyField(nameRect, sp, GUIContent.none);
                break;
            case AnimatorControllerParameterType.Float:
                EditorGUI.PropertyField(nameRect, property.FindPropertyRelative(nameof(AnimatorParams.value)), GUIContent.none);
                break;
            case AnimatorControllerParameterType.Trigger:
                EditorGUI.PropertyField(nameRect, property.FindPropertyRelative(nameof(AnimatorParams.value)), GUIContent.none);
                break;
            default:
                break;
        }
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
    public AnimatorControllerParameterType type; // float ,trigger,bool ,int
    public object value; // 1.0f ,true ,false ,1
    public AnimatorParams(string state, string param, AnimatorControllerParameterType type, object value)
    {
        this.state = state;
        this.param = param;
        this.type = type;
        this.value = value;
    }
    public override string ToString()
    {
        var info = @$"state = {state}
param = {param}
type = {type}
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