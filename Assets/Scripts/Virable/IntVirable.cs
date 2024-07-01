using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVirable", menuName = "Virable/IntVirable")]
public class IntVirable : ScriptableObject
{
    public int currentValue;
    public int maxValue;
    public IntEventSO valueChangEvent;//这个事件就是在value值变化的时候调用的事件 常用于更新UI
    [TextArea] 
    [SerializeField]private string VirDiscription;//Int事件描述

    public void setValue(int value) 
    {
        currentValue= value;
        valueChangEvent?.RaisEvent(value, this);
    }
}
