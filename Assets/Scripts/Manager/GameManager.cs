using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapLayoutSO layoutSO;
    public List<Enemy> enemies= new List<Enemy>();
    [Header("事件广播")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameloseEvent;
    public void UpdateMapLayout(object value) 
    {
        var currentRoomVector = (Vector2Int)value;
        if (layoutSO.roomMapdataList.Count == 0)
        {
            return;
        }
        var currentRoom = layoutSO.roomMapdataList.Find(r=>r.column==currentRoomVector.x&&r.line==currentRoomVector.y);
        
        currentRoom.roomState = RoomState.Visited;//已经进入过房间了改变状态
        //更新相邻房间的数据
        var sameColumnRooms = layoutSO.roomMapdataList.FindAll(r => r.column == currentRoom.column);
        foreach (var room in sameColumnRooms)
        {
            if (room.line!=currentRoomVector.y)
            {
                room.roomState = RoomState.Locked;
            }
           
        }
        foreach (var link in currentRoom.linkTo)
        {
            var linkRoom=layoutSO.roomMapdataList.FindAll(r=>r.column==link.x&&r.line==link.y);
            foreach (var room in linkRoom)
            {
                room.roomState = RoomState.Attainable;
            }
            
        }
        enemies.Clear();
    }
    public void OnRoomLoadEvent() 
    {
        var enemiess = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemiess)
        {
            enemies.Add(enemy);
        }

    }
    public void OnCharacterDeadEvent(object character) 
    {
        if (character is Player)
        {
            //玩家死亡游戏失败
            StartCoroutine(EventDelayAction(gameloseEvent));

        }
        if (character is BOSS)
        {
            StartCoroutine(EventDelayAction(gameloseEvent));
        }
        else if(character is Enemy)
        {
            enemies.Remove(character as Enemy);
            if (enemies.Count == 0)
            {
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
    }
    /// <summary>
    /// 延迟执行死亡之后的逻辑
    /// </summary>
    /// <param name="eventSO"></param>
    /// <returns></returns>
    IEnumerator EventDelayAction(ObjectEventSO eventSO) 
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaisEvent(null, this);
    }
    public void MapClear() 
    {
        layoutSO.roomMapdataList.Clear();
        layoutSO.roomLinedataList.Clear();
    }
}
