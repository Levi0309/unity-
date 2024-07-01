using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardListSO;//��Ϸ�п��ܳ��ֵ����п����б�

    public CardLibrarySO NewlibrarySO;//��ʼ��Ϸ��ʱ���ƿ����
    public CardLibrarySO PlayercardDataSO;//��ʼ��Ϸ��ʱ������е���

    private int previousIndex;
    
    private void Awake()
    {
        InitializeCardDataList();
        Debug.Log(PlayercardDataSO.cardDataEntries.Count);
        foreach (var item in NewlibrarySO.cardDataEntries)
        {
            PlayercardDataSO.cardDataEntries.Add(item);//�ȴ��ƿ��л�ȡ
        }
        Debug.Log(PlayercardDataSO.cardDataEntries.Count);
    }
    private void OnDisable()
    {
       PlayercardDataSO.cardDataEntries.Clear();
    }
    #region  ��Addressable���ؿ��ֿ�����Դ
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
    #region  �����Ŀ����
    //��ȡ
    public GameObject GetCardObj() 
    {
        var currentCard= poolTool.GetObjectFromPool();
        currentCard.transform.localScale= Vector3.zero;
        return currentCard;
    }
    //����
    public void DiscardCard(GameObject cardObj) 
    {
        poolTool.ReturnObjectToPool(cardObj);
    }
    #endregion
    /// <summary>
    /// ���������������ظ�
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
            //����Ѿ������������  �ҵ��������Ȼ������++
            var cardDataEntry= PlayercardDataSO.cardDataEntries.Find(t=>t.cardDataLi==card);
            cardDataEntry.amount++;

        }
        else 
        {
            //���û��  �б�������������
            PlayercardDataSO.cardDataEntries.Add(newCard);

        }
    }
}
