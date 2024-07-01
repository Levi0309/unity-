using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Effect", menuName = "Effect/DefenceEffent")]
public class DefenceEffent : CardEffect
{
    public override void Excute(CharacterBase from, CharacterBase to)
    {
       if(excuteType==EffectExcuteType.self)
       {
            from.updateDefence(value);

       }
       else if(excuteType==EffectExcuteType.Target)
       {
            to.updateDefence(value);

       }
    }
}
