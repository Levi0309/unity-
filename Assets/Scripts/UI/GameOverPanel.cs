using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private Button backBtn;
    public ObjectEventSO loadMenuEvent;
    private void OnEnable()
    {
        backBtn = GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton");
        backBtn.clicked += () => OnBackToMenu();
    }

    private void OnBackToMenu()
    {
        loadMenuEvent.RaisEvent(null, this);
    }
}
