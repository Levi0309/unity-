
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager cardManager;
    private VisualElement root;                             //��ǰ��常����
    public VisualTreeAsset CardTemplete;                    //����UIģ��
    private CardDataSO currentcardData;                     //��ǰѡ�еĿ�������
    private VisualElement Container;                        //װ���������ư�ť������(������)
    private List<Button> buttons= new();                    //��ǰ��ѡ���������ư�ť
    private Button ConfirmBtn;                              //ѡ�п���ȷ����ť

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
            InitCard(card, data);//��ʼ��
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
            CardType.Attack => "����",
            CardType.Deffend => "����",
            CardType.Abilities => "����",
            _ => throw new System.NotImplementedException()
        };
        Description.text = cardData.cardDecription.ToString();
        Cost.text = cardData.cardCost.ToString();
    }
}
