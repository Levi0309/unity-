using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO; //事件SO文件  作用:获得某个事件引用
    public UnityEvent<T> response;//事件的反馈  这个事件要订阅的方法是拖拽赋值的
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
        response.Invoke(value);//把事件全部都启动
    }
}
