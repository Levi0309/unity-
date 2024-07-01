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
        ResetDefence();//����ֻ��һ�غ�
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
        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f//normalizedTime ���Ա�ʾ�����ӿ�ʼ�������Ĺ�һ��ʱ�䣬�ڷ�Χ [0, 1] ��
        > 0.6);

        if (actionAction=="attack")
        {
            //����ǹ���
            currentAction.enemyEffect.Excute(this, player);
        }
        else 
        {
            currentAction.enemyEffect.Excute(this,this);
        }

    }



}
