using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// SO�ļ������¼������¼�  mono�ﶩ�ķ���
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;//�¼�����

    public UnityAction<T> OnEventRaised;//�����¼�

    public string lastSender;//ͨ��������� ���˭�㲥������¼�
    /// <summary>
    /// �¼�����
    /// </summary>
    /// <param name="value"></param>
    public void RaisEvent(T value,object sender) 
    {
        OnEventRaised?.Invoke(value);
        lastSender=sender.ToString();
    }
}
