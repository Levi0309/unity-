using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public Vector3 deckPos;
    public CardLayoutManager CardLayoutManager;
    private List<CardDataSO> drawDeck = new();//���ƶ�
    private List<CardDataSO> discardDeck= new();//���ƶ�
    private List<Card> handCardObjectList= new();//��ǰ�غϵ��µ����ƶ���Ϊ��ʵ����Ʒ����Card
    [Header("�㲥�¼�")]
    public IntEventSO drawEventSO;
    public IntEventSO discardEventSO;
    //������
    private void Start()
    {

        InitializeDeck();
       
        
    }

    [ContextMenu("��ȡ����")]
    public void TextDrawCard() 
    {
        DrawCard(1);
    }
    public void NewGameBeginDelay()
    {
        Invoke("NewGameBegin",0.1f);
    }
    public void NewGameBegin()//Ҫ����playermana��������ִֵ��!!!
    {
        InitializeDeck();
        DrawCard(4);
    }
    /// <summary>
    /// ���ƶ�����ƾ��ǿ������ƿ����
    /// </summary>
    private void InitializeDeck() 
    {
        drawDeck.Clear();
        foreach (var item in cardManager.PlayercardDataSO.cardDataEntries)
        {
            for (int i = 0; i < item.amount; i++)
            {
                drawDeck.Add(item.cardDataLi);
            }
        }
        //Todo ϴ��
        ShuffleDeck();
    }
    public void DrawCard(int amount) 
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count==0)
            {
                //���û������
                foreach (var disCard in discardDeck)
                {
                    drawDeck.Add(disCard);//�����ƶѼ���
                }
                ShuffleDeck(); //˵�����ƶ�û������ Ҫϴ����

            }
            CardDataSO currentCard = drawDeck[0];//�ӳ��ƶ�ȡ��һ�� ������
            drawDeck.RemoveAt(0);//Ȼ����ƶ��Ƴ�������
            drawEventSO.RaisEvent(drawDeck.Count, this);
            var card = cardManager.GetCardObj().GetComponent<Card>();//�Ӷ����ȡһ����
            card.Init(currentCard);
            card.transform.position = deckPos;
            handCardObjectList.Add(card);
            float delay = i * 0.2f;
            setCardPos(delay);//ÿ�����һ�����ƶ�ȫ����������һ��λ��
        }
       

    }
    /// <summary>
    /// �������ɵĿ���λ�ú���ת
    /// </summary>
    public void setCardPos(float delay) 
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            var currentCard = handCardObjectList[i];
            CardTransform cardTransform = CardLayoutManager.GetTransform(i, handCardObjectList.Count);
            currentCard.UpdateCardState();
            currentCard.isAnimationing = true;
            
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete=()=> 
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete=()=> { currentCard.isAnimationing = false; };
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };
            currentCard.GetComponent<SortingGroup>().sortingOrder= i+10;
            currentCard.UpdateOriginPosAndRotate(cardTransform.pos,cardTransform.rotation);
            
        }
    }
    /// <summary>
    /// ϴ�ƹ���
    /// </summary>
    private void ShuffleDeck() 
    {
        discardDeck.Clear();
        //����UI��ʾ����
        drawEventSO.RaisEvent(drawDeck.Count, this);
        discardEventSO.RaisEvent(discardDeck.Count, this);
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO cardData = drawDeck[i];
            int randomIndex=Random.Range(0,drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = cardData;
        }
    }
    /// <summary>
    /// �����߼�
    /// </summary>
    public void DiscradCard(object obj) 
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCard(card.gameObject);
        //ÿ�δ���һ���� ��������
        setCardPos(0);
    }
    /// <summary>
    /// ��һغϽ��� �������е�����
    /// </summary>
    public void PlayerTurnEndEvent()
    {
       for (int i = 0; i < handCardObjectList.Count; i++)
       {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
       }
       handCardObjectList.Clear();
       discardEventSO.RaisEvent(discardDeck.Count, this);
    }
    public void DisCardAllCard() 
    {
        foreach (var item in handCardObjectList)
        {
            cardManager.DiscardCard(item.gameObject);
        }
        handCardObjectList.Clear();
        InitializeDeck();//��ʼ�����ƿ�   ��Ϸʤ�����ֹ��һ����Ϸ���ƿ�û��
    }
    

}
