using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Effect", menuName = "Effect/StrengthEffent")]
public class StrengthEffect : CardEffect
{
    public override void Excute(CharacterBase from, CharacterBase to)
    {
       switch(excuteType)
       {
            case EffectExcuteType.self:
            from.SetupStrength(value,true);
            break;
            case EffectExcuteType.Target:
            to.SetupStrength(value,false);
            break;

       }
    }
}
