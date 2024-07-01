using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu; 
    public AssetReference intro;
    public Vector2Int currentRoomVector;
    private Room currentRoom;
    public ObjectEventSO AfterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;//�洢ÿ����������λ�õ��¼�
    public FadePanel fadePanel;
    private void Start()
    {
        currentRoomVector = Vector2Int.one * -1;        
        LoadIntro();//����TimeLine��������,Ȼ����ز˵�
        //LoadMenu();//ֱ�Ӽ��ز˵�
    }
    /// <summary>
    /// �ڷ����м����¼�����  �������Ҫת����Ӧ����
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data) 
    {
        if (data is Room)
        {
            currentRoom=data as Room;
            RoomData_SO currentData = currentRoom.roomData;
            // Debug.Log(currentData.roomType);
            currentRoomVector = new(currentRoom.column, currentRoom.line);
            currentScene = currentData.sceneToLoad;
        }
        //��ж�ص�ǰ����
        await UnLoadSceneTask();
        //���ط��䳡��
        await LoadSceneTask();

        AfterRoomLoadedEvent.RaisEvent(currentRoom, this);

    }
    /// <summary>
    /// �첽�������س���
    /// ����ֵAwaitable�ɵȴ���
    /// await scene.Task: ʹ�� await �ȴ��첽�������ʱ����ǰ�����ᱻ��ִͣ�У�ֱ���첽������ɡ����ַ�ʽ�����ڵȴ��첽������ͬʱִ����������
    /// scene.WaitForCompletion(): ����һ��ͬ����������������ǰ�̣߳�ֱ���첽������ɡ�����ζ���ڵ�����������󣬳����һֱ�ȴ���ֱ���첽������ɣ��ڼ��޷�ִ����������
    /// </summary>
    /// <returns></returns>
    private async Awaitable LoadSceneTask()
    {
        var scene= currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await scene.Task;
        if (scene.Status==AsyncOperationStatus.Succeeded)
        {
            fadePanel.FadeInScene(0.2f);
            SceneManager.SetActiveScene(scene.Result.Scene);
        }

    }
    /// <summary>
    /// �첽ж�س���
    /// </summary>
    /// <returns></returns>
    private async Awaitable UnLoadSceneTask() 
    {
        fadePanel.FadeOutScene(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));

    }
    /// <summary>
    /// ��������map
    /// </summary>
    public async void LoadMap() 
    {

        await UnLoadSceneTask();//��ж�ص�ǰ����
        if (currentRoomVector!= Vector2.one * -1)
        {
            updateRoomEvent.RaisEvent(currentRoomVector, this);
        }
        currentScene = map;
        await LoadSceneTask();
    }
    public async void LoadMenu()
    {
        if (currentScene!=null)
        {
            await UnLoadSceneTask();//��ж�ص�ǰ����
        }
       
        currentScene = menu;
        await LoadSceneTask();
    }
    public async void LoadIntro()
    {
        if (currentScene != null)
        {
            await UnLoadSceneTask();//��ж�ص�ǰ����
        }

        currentScene = intro;
        await LoadSceneTask();
    }
}
