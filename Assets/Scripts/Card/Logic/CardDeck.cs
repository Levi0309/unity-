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
    private List<CardDataSO> drawDeck = new();//抽牌堆
    private List<CardDataSO> discardDeck= new();//弃牌堆
    private List<Card> handCardObjectList= new();//当前回合底下的手牌堆因为是实际物品所以Card
    [Header("广播事件")]
    public IntEventSO drawEventSO;
    public IntEventSO discardEventSO;
    //测试用
    private void Start()
    {

        InitializeDeck();
       
        
    }

    [ContextMenu("抽取卡牌")]
    public void TextDrawCard() 
    {
        DrawCard(1);
    }
    public void NewGameBeginDelay()
    {
        Invoke("NewGameBegin",0.1f);
    }
    public void NewGameBegin()//要晚于playermana重新设置值执行!!!
    {
        InitializeDeck();
        DrawCard(4);
    }
    /// <summary>
    /// 抽牌堆里的牌就是开局里牌库的牌
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
        //Todo 洗牌
        ShuffleDeck();
    }
    public void DrawCard(int amount) 
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count==0)
            {
                //如果没有牌了
                foreach (var disCard in discardDeck)
                {
                    drawDeck.Add(disCard);//从弃牌堆加牌
                }
                ShuffleDeck(); //说明抽牌堆没有牌了 要洗牌了

            }
            CardDataSO currentCard = drawDeck[0];//从抽牌堆取第一张 给手牌
            drawDeck.RemoveAt(0);//然后抽牌堆移除这张牌
            drawEventSO.RaisEvent(drawDeck.Count, this);
            var card = cardManager.GetCardObj().GetComponent<Card>();//从对象池取一张牌
            card.Init(currentCard);
            card.transform.position = deckPos;
            handCardObjectList.Add(card);
            float delay = i * 0.2f;
            setCardPos(delay);//每次添加一个卡牌都全部重新设置一下位置
        }
       

    }
    /// <summary>
    /// 设置生成的卡牌位置和旋转
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
    /// 洗牌功能
    /// </summary>
    private void ShuffleDeck() 
    {
        discardDeck.Clear();
        //更新UI显示数量
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
    /// 弃牌逻辑
    /// </summary>
    public void DiscradCard(object obj) 
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCard(card.gameObject);
        //每次打完一张牌 重新排序
        setCardPos(0);
    }
    /// <summary>
    /// 玩家回合结束 回收所有的手牌
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
        InitializeDeck();//初始化抽牌库   游戏胜利后防止下一局游戏抽牌库没牌
    }
    

}
