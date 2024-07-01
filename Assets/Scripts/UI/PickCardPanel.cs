
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager cardManager;
    private VisualElement root;                             //当前面板父物体
    public VisualTreeAsset CardTemplete;                    //卡牌UI模板
    private CardDataSO currentcardData;                     //当前选中的卡牌数据
    private VisualElement Container;                        //装载三个卡牌按钮的容器(父物体)
    private List<Button> buttons= new();                    //当前可选的三个卡牌按钮
    private Button ConfirmBtn;                              //选中卡牌确定按钮

    public ObjectEventSO finishCardPick;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        Container=root.Q<VisualElement>("Container");
        ConfirmBtn=root.Q<Button>("ConfirmBtn");
        ConfirmBtn.clicked += () => OnConfirmCardClicked();
        for (int i = 0; i < 3; i++)
        {
            TemplateContainer card= CardTemplete.Instantiate();         
            CardDataSO data= cardManager.GetNewCardData();
            InitCard(card, data);//初始化
            Container.Add(card);

            var CardButton = card.Q<Button>("CardButton");
            buttons.Add(CardButton);
            CardButton.clicked +=()=> OnCardClicked(CardButton, data);
        }
    }

    private void OnConfirmCardClicked()
    {
        cardManager.UnLockedCard(currentcardData);
        finishCardPick.RaisEvent(null, this);
       
    }

    private void OnCardClicked(Button cardButton,CardDataSO data)
    {
        currentcardData=data;
        //Debug.Log(" currentcardData "+currentcardData.cardName);
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i]==cardButton)
            {
               
                buttons[i].SetEnabled(false);
            }
            else
            {
                buttons[i].SetEnabled(true);
            }
        }
    }

    public void InitCard(VisualElement card,CardDataSO cardData) 
    {
        var cardIcon = card.Q<VisualElement>("CardIcon");
        var Name = card.Q<Label>("Name");
        var Type = card.Q<Label>("Type");
        var Description = card.Q<Label>("Description");
        var Cost = card.Q<Label>("Cost");

        cardIcon.style.backgroundImage = new StyleBackground(cardData.cardImage);
        Name.text = cardData.cardName;
        Type.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Deffend => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException()
        };
        Description.text = cardData.cardDecription.ToString();
        Cost.text = cardData.cardCost.ToString();
    }
}
