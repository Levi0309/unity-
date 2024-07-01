using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// SO文件定义事件调用事件  mono里订阅方法
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;//事件描述

    public UnityAction<T> OnEventRaised;//具体事件

    public string lastSender;//通过这个变量 获得谁广播了这个事件
    /// <summary>
    /// 事件调用
    /// </summary>
    /// <param name="value"></param>
    public void RaisEvent(T value,object sender) 
    {
        OnEventRaised?.Invoke(value);
        lastSender=sender.ToString();
    }
}
