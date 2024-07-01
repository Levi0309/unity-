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
    public ObjectEventSO updateRoomEvent;//存储每个房间行列位置的事件
    public FadePanel fadePanel;
    private void Start()
    {
        currentRoomVector = Vector2Int.one * -1;        
        LoadIntro();//加载TimeLine动画开局,然后加载菜单
        //LoadMenu();//直接加载菜单
    }
    /// <summary>
    /// 在房间中加载事件监听  点击房间要转到对应场景
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
        //先卸载当前场景
        await UnLoadSceneTask();
        //加载房间场景
        await LoadSceneTask();

        AfterRoomLoadedEvent.RaisEvent(currentRoom, this);

    }
    /// <summary>
    /// 异步操作加载场景
    /// 返回值Awaitable可等待的
    /// await scene.Task: 使用 await 等待异步操作完成时，当前方法会被暂停执行，直到异步操作完成。这种方式允许在等待异步操作的同时执行其他任务。
    /// scene.WaitForCompletion(): 这是一个同步方法，会阻塞当前线程，直到异步操作完成。这意味着在调用这个方法后，程序会一直等待，直到异步操作完成，期间无法执行其他任务。
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
    /// 异步卸载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable UnLoadSceneTask() 
    {
        fadePanel.FadeOutScene(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));

    }
    /// <summary>
    /// 监听返回map
    /// </summary>
    public async void LoadMap() 
    {

        await UnLoadSceneTask();//先卸载当前场景
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
            await UnLoadSceneTask();//先卸载当前场景
        }
       
        currentScene = menu;
        await LoadSceneTask();
    }
    public async void LoadIntro()
    {
        if (currentScene != null)
        {
            await UnLoadSceneTask();//先卸载当前场景
        }

        currentScene = intro;
        await LoadSceneTask();
    }
}
