using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Effect", menuName = "Effect/DrawTwoCardEffect")]
public class DrawTwoCardEffect : CardEffect
{
    public IntEventSO drawTwoCardEvent;
    public override void Excute(CharacterBase from, CharacterBase to)
    {
       drawTwoCardEvent.RaisEvent(value,this);
    }

   
}
