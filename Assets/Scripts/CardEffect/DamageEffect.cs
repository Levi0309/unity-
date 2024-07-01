using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/DamageEffect")]
public class DamageEffect : CardEffect
{
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        if (target==null)
        {
            return;
        }
        switch (excuteType)
        {
            case EffectExcuteType.Target:
                var damage =Mathf.RoundToInt(from.baseStrength * value);
                target.TakeDamage(damage);
                break;
            case EffectExcuteType.All:
                foreach (var tar in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    tar.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
            default:
                break;
        }
    }
}
