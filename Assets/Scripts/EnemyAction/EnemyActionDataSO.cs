using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyActionDataSO", menuName = "Enemy/EnemyActionDataSO")]
public class EnemyActionDataSO : ScriptableObject
{
   public List<EnemyAction> enemyActions;
}
[System.Serializable]
public class EnemyAction 
{
    public Sprite IntentIcon;
    public CardEffect enemyEffect;
}
