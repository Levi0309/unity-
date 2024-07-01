using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement root;
    private Button backMapBtn;
    private Button selectedBtn;

    [Header("事件广播")]
    public ObjectEventSO LoadMapEvent;
    public ObjectEventSO PickCardEvent;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        backMapBtn = root.Q<Button>("BackBtn");
        selectedBtn = root.Q<Button>("SelectedCard");
        backMapBtn.clicked += OnBackToMapEvent;
        selectedBtn.clicked += OnPickCardMapEvent;
    }

    private void OnBackToMapEvent()
    {
        LoadMapEvent.RaisEvent(null, this);
        Debug.Log("返回地图!!!");
    }
    private void OnPickCardMapEvent() 
    {
        PickCardEvent.RaisEvent(null, this);
        Debug.Log("选牌!!!!!!!!!");
    }
    public void OnFnishCardPickEvent()
    {
        selectedBtn.style.display = DisplayStyle.None;
    }
}
