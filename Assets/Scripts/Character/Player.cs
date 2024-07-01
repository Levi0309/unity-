using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVirable playerMana;//Mana����ֵ

    public int maxMana;
    public int CurrentMana{get=>playerMana.currentValue;set=>playerMana.setValue(value);}//�������� ���ֵ�ĸı��Ӱ�쵽SO�ļ����ֵ�ĸı�
    private void OnEnable() 
    {
        playerMana.maxValue=maxMana;
        CurrentMana=playerMana.maxValue;
        
    }
    //�»غϿ�ʼʱ�����ص����ֵ
    public void NewTurn()
    {
        CurrentMana=maxMana;
        
    

    }
    public void UpdateManaCost(int cost)
    {
        CurrentMana-=cost;
        if(CurrentMana<=0)
        {
            CurrentMana=0;
        }

    }
    public void NewGame() 
    {
        Currenthp = maxHP;
        buffRound.currentValue = buffRound.maxValue;
        isDead=false;
        NewTurn();
    }
   
}
