using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject PickCardPanel;
    public GameObject RestRoomPanel;
    public  void OnLoadRoomEvent(object data) 
    {
        Room room= (Room)data;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                RestRoomPanel.SetActive(true);
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// 加载地图的时候全隐藏
    /// </summary>
    public void HideAllPanels() 
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        RestRoomPanel.SetActive(false);
    }
    public void OnGameWinEvent()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }
    public void OnGameLoseEvent() 
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void OnPickCardEvent()
    {
        PickCardPanel.SetActive(true);
    }
    public void OnFnishCardPickEvent()
    {
        PickCardPanel.SetActive(false);
    }
}
