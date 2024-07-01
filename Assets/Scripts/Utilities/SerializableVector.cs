using UnityEngine;

[System.Serializable]
public class SerializableVector
{
   public float x, y,z;
   public SerializableVector(float posx, float posy, float posz) 
   {
        x= posx;
        y= posy;
        z= posz;
   }
    public Vector3 ToVector3() 
    {
        return new Vector3(x,y,z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}
