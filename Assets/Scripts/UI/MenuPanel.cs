using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button QuitBtn, NewGameBtn;
    public ObjectEventSO NewGameEvent;
    private void OnEnable()
    {
        
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        QuitBtn = rootElement.Q<Button>("QuitGame");
        NewGameBtn= rootElement.Q<Button>("NewGame");
        QuitBtn.clicked += () => Application.Quit();
        NewGameBtn.clicked += () => OnNewGame();
    }
    private void OnNewGame() 
    {
        NewGameEvent.RaisEvent(null, this);

    }
}
