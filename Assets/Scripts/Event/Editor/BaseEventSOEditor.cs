using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ���Զ���༭�������� BaseEventSO<> ���͵�����ʵ���ϡ�����ζ�ţ������� Unity �༭���м���༭ 
/// BaseEventSO<> ���͵Ķ���ʱ������ʹ��ָ�����Զ���༭�����������ǣ�������Ĭ�ϵı༭����
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
    /// Editor�Զ���������
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("��������: "+ GetListeners().Count);
        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());//��ʾ������������
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
