using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Room roomPrefab;
    public LineRenderer line;
    public MapConfingeSO mapConfin;
    [Header("保存房子布局")]
    public MapLayoutSO mapLayout;
    [Header("生成房子")]
    private float screenHeight;   
    private float screenWidth;
    private float columnWidth;
    [SerializeField]private float border;


    private Vector3 generatorPoint;//记录生成房间的位置
    private List<Room> roomList= new();//每次切换场景都会new 都是0 然后再加载房间 添加进这个列表 不会重复添加
    private List<LineRenderer> lines = new();//记录当前的所有连线  地图更新后删除所有连线
    public List<RoomData_SO> roomDataList = new();
    private Dictionary<RoomType, RoomData_SO> roomDict = new();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize*2;
        screenWidth = screenHeight * Camera.main.aspect;
        columnWidth=screenWidth/(mapConfin.rooms.Count);//横向间隙
        foreach (RoomData_SO roomData in roomDataList)
        {
            roomDict.Add(roomData.roomType, roomData);
        }
    }
    private void OnEnable()
    {
        if (mapLayout.roomMapdataList.Count>0)
        {
            LoadMap();
        }
        else 
        {
            CreatMap();
        }
        
    }

  
    private void CreatMap() 
    {
        List<Room> previousColumnRoom = new();

        for (int column = 0; column < mapConfin.rooms.Count; column++)
        {
            List<Room> currentColumnRoom = new();
            RoomBlueMap bluePrint = mapConfin.rooms[column];
            int amount =UnityEngine.Random.Range(bluePrint.min,bluePrint.max);
            float startHeight=screenHeight/2-screenHeight/(amount+1);//screenHeight/(amount+1);最顶上的位置减去间隙 给上面留空
            generatorPoint =new Vector3(-screenWidth/2+border+columnWidth*column,startHeight,0);
            Vector3 newPosition= generatorPoint;
            float GapY= screenHeight / (amount + 1);
            for (int i = 0; i < amount; i++)
            {
                if (column==mapConfin.rooms.Count-1)//如果是最后一列房子 说明是boss 我们让他靠右
                {
                    newPosition.x = screenWidth / 2 - border*1.2f;
                }
                else if (column !=0) 
                {
                    //否则如果不等于第一列
                    //我们让他x都有一点偏移
                    newPosition.x = generatorPoint.x + UnityEngine.Random.Range(-border/2,border/2);
                }
                newPosition.y=startHeight-GapY*i;
                //生成房间
                Room room=  Instantiate(roomPrefab,newPosition, Quaternion.identity,transform);
                RoomType roomType = GetRandomRoomType(mapConfin.rooms[column].roomType);
                RoomData_SO roomData = GetRoomData(roomType);
                if (column==0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else 
                {
                    room.roomState = RoomState.Locked;
                }
                room.SetupRoom(column, i, roomData);

                roomList.Add(room);
                currentColumnRoom.Add(room);
            }
            
            if (previousColumnRoom.Count>0)//说明已经遍历到第二列了  
            {
                GeneratorLines(previousColumnRoom, currentColumnRoom);
            }
            
            previousColumnRoom = currentColumnRoom;//遍历完第一列后这个previousColumnRoom才赋值
        }
        SaveMap();

    }

    /// <summary>
    /// 生成连线
    /// </summary>
    /// <param name="previousColumnRoom"></param>
    /// <param name="currentColumnRoom"></param>
    private void GeneratorLines(List<Room> previousColumnRoom, List<Room> currentColumnRoom)
    {
        HashSet<Room> currentLinedRoom = new HashSet<Room>();//当前已经连线的房间
        foreach (Room room in previousColumnRoom)
        {
            Room curroom = GetHasLinesRoom(room, currentColumnRoom, false);
            currentLinedRoom.Add(curroom);
        }
        foreach (Room room in currentColumnRoom)//反向连线
        {
            if (!currentLinedRoom.Contains(room))
            {
                Room curroom = GetHasLinesRoom(room, previousColumnRoom, true);
                currentLinedRoom.Add(curroom);
            }

        }
    }
    /// <summary>
    /// 得到已经连线的房间
    /// </summary>
    /// <param name="room"></param>
    /// <param name="currentRoom"></param>
    /// <returns></returns>
    private Room GetHasLinesRoom(Room room, List<Room> currentRoom, bool check)
    {
        Room targetRoom;
        targetRoom = currentRoom[UnityEngine.Random.Range(0, currentRoom.Count)];
        if (check) //反向连接
        {

            targetRoom.linkTo.Add(new(room.column, room.line));
        }
        else
        {
            room.linkTo.Add(new(targetRoom.column, targetRoom.line));
        }
        LineRenderer curLine = Instantiate(line, transform);
        curLine.SetPosition(0, room.transform.position);
        curLine.SetPosition(1, targetRoom.transform.position);

        lines.Add(curLine);
        return targetRoom;
    }
    /// <summary>
    /// 保存地图布局
    /// </summary>
    private void SaveMap()
    {
        mapLayout.roomMapdataList = new();
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomMapData roomMap = new RoomMapData()
            {
                posX = roomList[i].transform.position.x,
                posY = roomList[i].transform.position.y,
                column = roomList[i].column,
                line = roomList[i].line,
                roomData = roomList[i].roomData,
                roomState= roomList[i].roomState,
                linkTo= roomList[i].linkTo,
            };
            mapLayout.roomMapdataList.Add(roomMap);

        }

        mapLayout.roomLinedataList= new();
        for (int i = 0; i < lines.Count; i++)
        {
            RoomLineData roomLine = new RoomLineData()
            {
                StartPos = new SerializableVector(lines[i].GetPosition(0).x, lines[i].GetPosition(0).y, 0),
                EndPos = new SerializableVector(lines[i].GetPosition(1).x, lines[i].GetPosition(1).y, 0),
            };
            mapLayout.roomLinedataList.Add(roomLine);
        }
    }
    private void LoadMap()
    {
       
        for (int i = 0; i < mapLayout.roomMapdataList.Count; i++)
        {
            RoomMapData roomMap= mapLayout.roomMapdataList[i];            
            Vector3 newPos = new Vector3(roomMap.posX, roomMap.posY);
            Room newRoom = Instantiate(roomPrefab, newPos,Quaternion.identity, transform);
            newRoom.SetupRoom(roomMap.column, roomMap.line, roomMap.roomData);
            newRoom.roomState = roomMap.roomState;
            newRoom.linkTo= roomMap.linkTo;
            roomList.Add(newRoom);//因为roomList每次切换场景都会清零所以不存在重复添加

        }
        
        for (int i = 0; i < mapLayout.roomLinedataList.Count; i++)
        {
            RoomLineData roomLine = mapLayout.roomLinedataList[i];
            LineRenderer newline = Instantiate(line, transform);
            newline.SetPosition(0, roomLine.StartPos.ToVector3());
            newline.SetPosition(1,roomLine.EndPos.ToVector3());
            lines.Add(newline);
        }
    }

    [ContextMenu("GenerateRoom")]
    private void GenerateRoom() 
    {
        foreach (Room item in roomList)
        {
            Destroy(item.gameObject);
        }
        foreach (LineRenderer item in lines)
        {
            Destroy(item.gameObject);
        }
        roomList.Clear();
        lines.Clear();
        CreatMap();
    }
    /// <summary>
    /// 根据房间类型 获得房间SO数据   后面可以根据房间数据SetUp每个房间
    /// </summary>
    /// <param name="roomType"></param>
    /// <returns></returns>
    public RoomData_SO GetRoomData(RoomType roomType) 
    {
        return roomDict[roomType];
    }
    /// <summary>
    /// 根据传入的RoomType返回,因为有好多flag 随机返回其中一个roomtype
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public RoomType GetRandomRoomType(RoomType flag) 
    {
        string[] options = flag.ToString().Split(",");//获得好多RoomType类型字符串
        string option = options[UnityEngine.Random.Range(0, options.Length)];
        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), option);
        return roomType;

    }
}
