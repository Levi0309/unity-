using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVirable playerMana;//Mana法力值

    public int maxMana;
    public int CurrentMana{get=>playerMana.currentValue;set=>playerMana.setValue(value);}//定义属性 这个值的改变会影响到SO文件里的值的改变
    private void OnEnable() 
    {
        playerMana.maxValue=maxMana;
        CurrentMana=playerMana.maxValue;
        
    }
    //新回合开始时法力回到最大值
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
