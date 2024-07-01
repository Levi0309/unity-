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
    [Header("�㲥")]
    public ObjectEventSO loadRoomEvent;
    public List<Vector2Int> linkTo = new();//������������
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
        if (roomState==RoomState.Attainable)//������Խ����ٽ���
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
