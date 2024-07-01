using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public int value;
    public EffectExcuteType excuteType;
    public abstract void Excute(CharacterBase from, CharacterBase to);
}
