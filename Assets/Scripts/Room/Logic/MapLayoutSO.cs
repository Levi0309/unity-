using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="MapLayoutSO",menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<RoomMapData> roomMapdataList=new();
    public List<RoomLineData> roomLinedataList=new();
}
[Serializable]
public class RoomMapData
{
    //ÐÐ,ÁÐ,x,y
    public float posX,posY;
    public int column;
    public int line;
    public RoomData_SO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo;
}
[Serializable]
public class RoomLineData
{
    public SerializableVector StartPos,EndPos;
}
