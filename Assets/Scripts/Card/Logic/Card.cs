using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public SpriteRenderer cardsp;
    public TextMeshPro cardName, typeText, DescriptionText, costText;
    public CardDataSO cardData;
    public bool isAvilable;


    [Header("卡牌原始数据")]
    public Vector3 OriginCardPos;
    public Quaternion OriginCardRotate;
    public int OriginorderInLayer;
    public bool isAnimationing;
    public Player player;


    [Header("广播事件")]
    public ObjectEventSO disCardEvent;//回收卡牌
    public IntEventSO costManaEvent;
    private void Start()
    {
        Init(cardData);
    }
    public void Init(CardDataSO Data) 
    {
        cardData = Data;
        cardName.text=Data.cardName.ToString();
        cardsp.sprite = Data.cardImage;     
        DescriptionText.text = Data.cardDecription.ToString();
        costText.text = Data.cardCost.ToString();
        typeText.text = Data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Deffend => "技能",
            CardType.Abilities =>"能力",
            _ => throw new System.NotImplementedException(),
        };
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void UpdateOriginPosAndRotate(Vector3 pos, Quaternion rotate)
    {
        OriginCardPos = pos;
        OriginCardRotate = rotate;
        OriginorderInLayer = GetComponent<SortingGroup>().sortingOrder; 
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimationing)
        {
            return;
        }
        transform.position =OriginCardPos+ Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimationing)
        {
            return;
        }
        ResetPosAndRo();
    }

    public void ResetPosAndRo()
    {
        transform.SetPositionAndRotation(OriginCardPos, OriginCardRotate);
        GetComponent<SortingGroup>().sortingOrder = OriginorderInLayer;
    }
    public void ExcuteCardEffect(CharacterBase from,CharacterBase to) 
    {
        costManaEvent.RaisEvent(cardData.cardCost,this);
        disCardEvent.RaisEvent(this, this);
        
        foreach (var effect in cardData.cardEffects)
        {
            effect.Excute(from, to);
        }
    }
    public void UpdateCardState()
    {
        isAvilable=cardData.cardCost<=player.CurrentMana;
        costText.color=isAvilable?Color.green:Color.red;

    }
}
