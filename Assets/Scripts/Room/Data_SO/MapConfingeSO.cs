using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfingeSO", menuName = "Map/MapConfingeSO")]
public class MapConfingeSO : ScriptableObject
{
   public List<RoomBlueMap> rooms;
}
[System.Serializable]
public class RoomBlueMap
{ 
    public int min, max;
    public RoomType roomType;
}
