using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{

    public int maxHP;
    protected Animator anim;
    public IntVirable hp;
    public int Currenthp { get=>hp.currentValue;set=>hp.setValue(value); }
    public int MaxHP { get => hp.maxValue; }
    public bool isDead;
    public IntVirable Defence;
    public VFXController vFX;
    //力量相关
    public float baseStrength=1f;
    private float strengthEffect=0.5f;
    public IntVirable buffRound;
    public ObjectEventSO GameDeadEvent;
    protected virtual void Awake() 
    {
        anim=GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        hp.maxValue = maxHP;
        Currenthp = MaxHP;
        buffRound.currentValue=buffRound.maxValue;//开始游戏增益回合归零
        ResetDefence();
       
    }
    public virtual void TakeDamage(int damage) 
    {
        int currentDamage=(damage-Defence.currentValue)>=0?damage-Defence.currentValue:0;
        int currentDefence=(damage-Defence.currentValue)>=0?0:Defence.currentValue-damage;//设置当前防御值
        Defence.setValue(currentDefence);
        Debug.Log(currentDamage);
        if (Currenthp>currentDamage)
        {
            Currenthp -= currentDamage;
            Debug.Log(Currenthp);
            anim.SetTrigger("hit");
        }
        else 
        {
            Currenthp= 0;
            isDead = true;
            anim.SetBool("isDead", true);
            GameDeadEvent.RaisEvent(this, this);
        }
       
    }
     public void ResetDefence()
    {
        Defence.setValue(0);

    }
    public void updateDefence(int amount)
    {
        int defence=Defence.currentValue+amount;
        Defence.setValue(defence);

    }
    
    public void HealHealth(int amount)
    {
        int healAmount=hp.currentValue+amount;
        healAmount=Mathf.Min(maxHP,healAmount);
        hp.setValue(healAmount);
        vFX.Buff.SetActive(true);
    
    }
    /// <summary>
    /// 第二个参数代表 对敌人使用减少敌人伤害
    /// </summary>
    public void SetupStrength(int round,bool isPositive)
    {
        if(isPositive)
        {
            float newStrengh=baseStrength+strengthEffect;
            baseStrength=Mathf.Min(newStrengh,1.5f);//最高一点五倍伤害
            vFX.Buff.SetActive(true);

        }
        else
        {
            baseStrength=1-strengthEffect;
            vFX.DeBuff.SetActive(true);
        }
        int currentRound=buffRound.currentValue+round;
        if(baseStrength==1)//后面设置一张牌 清除所有Debuff
        {
             buffRound.setValue(0);
        }
        else
        {
            buffRound.setValue(currentRound);
        }
        
    }
    public void NextTurnSetStrength()
    {
        buffRound.currentValue-=1;
        if(buffRound.currentValue<=0)
        {
            buffRound.setValue(0);
            baseStrength=1;
        }


    }
}
