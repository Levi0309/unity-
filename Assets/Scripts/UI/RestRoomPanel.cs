using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button backtoMap, haveaBreak;
    public CardEffect healEffect;
    private Player Player;
    public ObjectEventSO LoadMap;
    private void Start()
    {
        Player=FindAnyObjectByType<Player>(FindObjectsInactive.Include);
        rootElement= GetComponent<UIDocument>().rootVisualElement;
        backtoMap = rootElement.Q<Button>("BackToMapBtn");
        haveaBreak = rootElement.Q<Button>("RestBtn");
        backtoMap.clicked += () => LoadMapEvent();
        haveaBreak.clicked += () => HaveABreak();
    }
    public void LoadMapEvent() 
    {
        LoadMap.RaisEvent(null, this);
    }
    public void HaveABreak() 
    {
        healEffect.Excute(Player, null);
        haveaBreak.SetEnabled(false);
    }
}
