using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 将自定义编辑器关联到 BaseEventSO<> 类型的所有实例上。这意味着，当你在 Unity 编辑器中检查或编辑 
/// BaseEventSO<> 类型的对象时，将会使用指定的自定义编辑器来呈现它们，而不是默认的编辑器。
/// </summary>
/// <typeparam name="T"></typeparam>
[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
   private BaseEventSO<T> baseEventSO;
    private void OnEnable()
    {
        if (baseEventSO==null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
    }
    /// <summary>
    /// Editor自定义检视面板
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("订阅数量: "+ GetListeners().Count);
        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());//显示监视器的名称
        }
    }
    private List<MonoBehaviour> GetListeners() 
    {
        List<MonoBehaviour> listeners = new();

        if (baseEventSO==null||baseEventSO.OnEventRaised==null)
        {
            return listeners;
        }

        var subscribes=baseEventSO.OnEventRaised.GetInvocationList();

        foreach (var subscribe in subscribes)
        {
            var obj = subscribe.Target as MonoBehaviour;
            if (!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }
        return listeners;
    }
}
