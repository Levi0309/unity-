using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Room roomPrefab;
    public LineRenderer line;
    public MapConfingeSO mapConfin;
    [Header("���淿�Ӳ���")]
    public MapLayoutSO mapLayout;
    [Header("���ɷ���")]
    private float screenHeight;   
    private float screenWidth;
    private float columnWidth;
    [SerializeField]private float border;


    private Vector3 generatorPoint;//��¼���ɷ����λ��
    private List<Room> roomList= new();//ÿ���л���������new ����0 Ȼ���ټ��ط��� ��ӽ�����б� �����ظ����
    private List<LineRenderer> lines = new();//��¼��ǰ����������  ��ͼ���º�ɾ����������
    public List<RoomData_SO> roomDataList = new();
    private Dictionary<RoomType, RoomData_SO> roomDict = new();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize*2;
        screenWidth = screenHeight * Camera.main.aspect;
        columnWidth=screenWidth/(mapConfin.rooms.Count);//�����϶
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
            float startHeight=screenHeight/2-screenHeight/(amount+1);//screenHeight/(amount+1);��ϵ�λ�ü�ȥ��϶ ����������
            generatorPoint =new Vector3(-screenWidth/2+border+columnWidth*column,startHeight,0);
            Vector3 newPosition= generatorPoint;
            float GapY= screenHeight / (amount + 1);
            for (int i = 0; i < amount; i++)
            {
                if (column==mapConfin.rooms.Count-1)//��������һ�з��� ˵����boss ������������
                {
                    newPosition.x = screenWidth / 2 - border*1.2f;
                }
                else if (column !=0) 
                {
                    //������������ڵ�һ��
                    //��������x����һ��ƫ��
                    newPosition.x = generatorPoint.x + UnityEngine.Random.Range(-border/2,border/2);
                }
                newPosition.y=startHeight-GapY*i;
                //���ɷ���
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
            
            if (previousColumnRoom.Count>0)//˵���Ѿ��������ڶ�����  
            {
                GeneratorLines(previousColumnRoom, currentColumnRoom);
            }
            
            previousColumnRoom = currentColumnRoom;//�������һ�к����previousColumnRoom�Ÿ�ֵ
        }
        SaveMap();

    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="previousColumnRoom"></param>
    /// <param name="currentColumnRoom"></param>
    private void GeneratorLines(List<Room> previousColumnRoom, List<Room> currentColumnRoom)
    {
        HashSet<Room> currentLinedRoom = new HashSet<Room>();//��ǰ�Ѿ����ߵķ���
        foreach (Room room in previousColumnRoom)
        {
            Room curroom = GetHasLinesRoom(room, currentColumnRoom, false);
            currentLinedRoom.Add(curroom);
        }
        foreach (Room room in currentColumnRoom)//��������
        {
            if (!currentLinedRoom.Contains(room))
            {
                Room curroom = GetHasLinesRoom(room, previousColumnRoom, true);
                currentLinedRoom.Add(curroom);
            }

        }
    }
    /// <summary>
    /// �õ��Ѿ����ߵķ���
    /// </summary>
    /// <param name="room"></param>
    /// <param name="currentRoom"></param>
    /// <returns></returns>
    private Room GetHasLinesRoom(Room room, List<Room> currentRoom, bool check)
    {
        Room targetRoom;
        targetRoom = currentRoom[UnityEngine.Random.Range(0, currentRoom.Count)];
        if (check) //��������
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
    /// �����ͼ����
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
            roomList.Add(newRoom);//��ΪroomListÿ���л����������������Բ������ظ����

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
    /// ���ݷ������� ��÷���SO����   ������Ը��ݷ�������SetUpÿ������
    /// </summary>
    /// <param name="roomType"></param>
    /// <returns></returns>
    public RoomData_SO GetRoomData(RoomType roomType) 
    {
        return roomDict[roomType];
    }
    /// <summary>
    /// ���ݴ����RoomType����,��Ϊ�кö�flag �����������һ��roomtype
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public RoomType GetRandomRoomType(RoomType flag) 
    {
        string[] options = flag.ToString().Split(",");//��úö�RoomType�����ַ���
        string option = options[UnityEngine.Random.Range(0, options.Length)];
        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), option);
        return roomType;

    }
}
