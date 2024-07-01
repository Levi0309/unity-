using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomData_SO roomData;
    public RoomState roomState;
    [Header("广播")]
    public ObjectEventSO loadRoomEvent;
    public List<Vector2Int> linkTo = new();//房间连线数据
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        SetupRoom(column, line,roomData);
    }

    private void OnMouseDown()
    {
        if (roomState==RoomState.Attainable)//房间可以进入再进入
        {
            loadRoomEvent.RaisEvent(this, this);
        }
       
    }
    public void SetupRoom(int _column,int _line,RoomData_SO _roomData) 
    {
        column = _column;
        line= _line;
        roomData = _roomData;
        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 1),
            RoomState.Attainable => new Color(1, 1, 1, 1),
            _ => throw new System.NotImplementedException(),
        };
    }
}
