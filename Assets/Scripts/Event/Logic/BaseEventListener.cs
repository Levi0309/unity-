using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO; //�¼�SO�ļ�  ����:���ĳ���¼�����
    public UnityEvent<T> response;//�¼��ķ���  ����¼�Ҫ���ĵķ�������ק��ֵ��
    private void OnEnable()
    {
        if (eventSO!=null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }
    private void OnDisable() 
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised -= OnEventRaised;
        }
    }

    private void OnEventRaised(T value)
    {
        response.Invoke(value);//���¼�ȫ��������
    }
}
