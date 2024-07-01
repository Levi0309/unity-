using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardTransform
{
    public Vector3 pos;
    public Quaternion rotation;
   public CardTransform(Vector3 _pos,Quaternion _rotation) 
   {
        pos = _pos;
        rotation = _rotation;
   }
}
