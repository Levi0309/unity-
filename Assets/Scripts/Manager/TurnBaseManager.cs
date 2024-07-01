using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{

    private bool playerTurn=false;
    private bool enemyTurn=false;
    public bool battleEnd=true;
    private float timeCounter;
    public float playerTurnDuration;//���ʱ������Ҳ��ſ�ʼ������ʱ��
    public float enemyTurnDuration;
    [Header("�㲥�¼�")]
    public ObjectEventSO playerTurnBeginSO;
    public ObjectEventSO enemyTurnBeginSO;
    public ObjectEventSO enemyTurnEndSO;
    public GameObject playObj;
    [ContextMenu("��Ϸ��ʼ")]
    private void GameStart()
    {
        playerTurn = true;
        enemyTurn = false;
        battleEnd =false;
        timeCounter=0;
    }
    void Update()
    {
        if(battleEnd==true)
        {
            return;
        }  
        if(enemyTurn)
        {
            timeCounter+=Time.deltaTime;
            if(timeCounter>enemyTurnDuration)
            {
                timeCounter=0;
                //���˻غϽ���
                //��һغ�׼����ʼ
                enemyTurnEnd();
                playerTurn=true;
                
            }
        }
        if(playerTurn)
        {
            timeCounter+=Time.deltaTime;
            if(timeCounter>playerTurnDuration)
            {
                timeCounter=0;
                //��ҿ�ʼ�ж�
                PlayerTurnBegin();
                playerTurn=false;
            }
        }

    }
    
    public void PlayerTurnBegin()
    {
        playerTurn=true;
        playerTurnBeginSO.RaisEvent(null,this);

    }
    public void enemyTurnBegin()
    {
        enemyTurn=true;
        enemyTurnBeginSO.RaisEvent(null,this);
    }
    public void enemyTurnEnd()
    {
        enemyTurn=false;
        enemyTurnEndSO.RaisEvent(null,this);

    }
    public void OnRoomLoadEvent(object value) 
    {
        Room room = (Room)value;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                playObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playObj.SetActive(true);
                playObj.GetComponent<PlayerAnimation>().PlayAnimSleep();
                break;
            default:
                break;
        }
    }
    public void LoadMapEvent() 
    {
        battleEnd = true;
        playObj.SetActive(false);
    }
    public void NewGame() 
    {
        playObj.GetComponent<Player>().NewGame();
    }
}
