using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVirable", menuName = "Virable/IntVirable")]
public class IntVirable : ScriptableObject
{
    public int currentValue;
    public int maxValue;
    public IntEventSO valueChangEvent;//����¼�������valueֵ�仯��ʱ����õ��¼� �����ڸ���UI
    [TextArea] 
    [SerializeField]private string VirDiscription;//Int�¼�����

    public void setValue(int value) 
    {
        currentValue= value;
        valueChangEvent?.RaisEvent(value, this);
    }
}
