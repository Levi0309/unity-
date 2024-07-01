using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardListSO;//游戏中可能出现的所有卡牌列表

    public CardLibrarySO NewlibrarySO;//开始游戏的时候牌库的牌
    public CardLibrarySO PlayercardDataSO;//开始游戏的时候玩家有的牌

    private int previousIndex;
    
    private void Awake()
    {
        InitializeCardDataList();
        Debug.Log(PlayercardDataSO.cardDataEntries.Count);
        foreach (var item in NewlibrarySO.cardDataEntries)
        {
            PlayercardDataSO.cardDataEntries.Add(item);//先从牌库中获取
        }
        Debug.Log(PlayercardDataSO.cardDataEntries.Count);
    }
    private void OnDisable()
    {
       PlayercardDataSO.cardDataEntries.Clear();
    }
    #region  从Addressable加载开局卡牌资源
    private void InitializeCardDataList() 
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle) 
    {
        if (handle.Status==AsyncOperationStatus.Succeeded)
        {
            cardListSO=new List<CardDataSO>(handle.Result);
        }
        else 
        {
            Debug.LogError("No Found Card!!!");
        }
    }
    #endregion
    #region  获得项目卡牌
    //获取
    public GameObject GetCardObj() 
    {
        var currentCard= poolTool.GetObjectFromPool();
        currentCard.transform.localScale= Vector3.zero;
        return currentCard;
    }
    //回收
    public void DiscardCard(GameObject cardObj) 
    {
        poolTool.ReturnObjectToPool(cardObj);
    }
    #endregion
    /// <summary>
    /// 避免相邻两张牌重复
    /// </summary>
    /// <returns></returns>
    public CardDataSO GetNewCardData() 
    {
        int randomIndex = 0;
        do
        {
            randomIndex=Random.Range(0,cardListSO.Count);
        } while (previousIndex == randomIndex);
        previousIndex = randomIndex;
        return cardListSO[previousIndex];
    }
    public void UnLockedCard(CardDataSO card) 
    {
        var newCard = new cardDataEntry()
        {
            cardDataLi = card,
            amount = 1
        };
        if (PlayercardDataSO.cardDataEntries.Contains(newCard))
        {
            //如果已经有这个卡牌了  找到这个卡牌然后数量++
            var cardDataEntry= PlayercardDataSO.cardDataEntries.Find(t=>t.cardDataLi==card);
            cardDataEntry.amount++;

        }
        else 
        {
            //如果没有  列表里添加这个数据
            PlayercardDataSO.cardDataEntries.Add(newCard);

        }
    }
}
