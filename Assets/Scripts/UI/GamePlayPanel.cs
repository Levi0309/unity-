using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    public ObjectEventSO playerTurnEndSO;
    private VisualElement rootvisualElement;
    private Label drawLable, disCardLable, energyLable, turnLable;
    private Button endTurnBtn;
    private void OnEnable()
    {
        rootvisualElement = GetComponent<UIDocument>().rootVisualElement;
        drawLable = rootvisualElement.Q<Label>("DrawAmount");
        energyLable = rootvisualElement.Q<Label>("EnergyAmount");
        disCardLable = rootvisualElement.Q<Label>("DiscardAmount");
        turnLable = rootvisualElement.Q<Label>("TurnLable");
        endTurnBtn = rootvisualElement.Q<Button>("EndTurn");
        endTurnBtn.clicked+=OnPlayerTurnEnd;

        drawLable.text = "0";
        disCardLable.text = "0";
        energyLable.text = "0";
        turnLable.text = "游戏开始";

    }

    private void OnPlayerTurnEnd()
    {
       playerTurnEndSO.RaisEvent(null,this);
    }

    public void UpdateDrawcardAmount(int amount)
    {
        drawLable.text = amount.ToString();
    }
    public void UpdateDiscardAmount(int amount)
    {
        disCardLable.text = amount.ToString();
    }
    public void UpdatePlayerMana(int amount)
    {
        energyLable.text=amount.ToString();
    }
    public void OnEnemyTurnBegin()
    {
        turnLable.text="敌人回合";
        endTurnBtn.SetEnabled(false);
        turnLable.style.color=new StyleColor(Color.red);

    }
    public void OnPlayerTurnBegin()
    {
        turnLable.text="玩家回合";
        endTurnBtn.SetEnabled(true);
        turnLable.style.color=new StyleColor(Color.white);

    }
    
}
