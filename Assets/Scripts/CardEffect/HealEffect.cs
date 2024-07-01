using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/HealEffent")]
public class HealEffect : CardEffect
{
    public override void Excute(CharacterBase from, CharacterBase to)
    {
       if(excuteType==EffectExcuteType.self)
       {
            from.HealHealth(value);
       }
       else if(excuteType==EffectExcuteType.Target)//敌人给其他敌人回血 用于多个敌人的情况
       {
            to.HealHealth(value);
       }
    }
}
