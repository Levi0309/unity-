using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO enemyAction;
    public EnemyAction currentAction;
    protected Player player;
 
    public void SetRandomAction() 
    {       
        int index= Random.Range(0, enemyAction.enemyActions.Count);
        currentAction= enemyAction.enemyActions[index];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void OnEnemyTurnBegin() 
    {
        ResetDefence();//防御只有一回合
        switch (currentAction.enemyEffect.excuteType)
        {
            case EffectExcuteType.self:
                Skill();
                break;
            case EffectExcuteType.Target:
                Attack();
                break;
            case EffectExcuteType.All:
                break;
            default:
                break;
        }
    }
    public void Attack() 
    {
        StartCoroutine(ActionDelay("attack"));
       
    }
    public void Skill() 
    {
        StartCoroutine(ActionDelay("skill"));
    }
    private IEnumerator ActionDelay(string actionAction)
    {
        anim.SetTrigger(actionAction);
        yield return new WaitUntil(() => anim.IsInTransition(0) 
        && anim.GetCurrentAnimatorStateInfo(0).IsName(actionAction) 
        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f//normalizedTime 属性表示动画从开始到结束的归一化时间，在范围 [0, 1] 内
        > 0.6);

        if (actionAction=="attack")
        {
            //如果是攻击
            currentAction.enemyEffect.Excute(this, player);
        }
        else 
        {
            currentAction.enemyEffect.Excute(this,this);
        }

    }



}
