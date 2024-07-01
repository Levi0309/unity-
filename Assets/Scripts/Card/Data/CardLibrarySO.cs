using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardLibrarySO ")]
public class CardLibrarySO : ScriptableObject
{
    public List<cardDataEntry> cardDataEntries;
}
[Serializable]
public class cardDataEntry 
{
   public CardDataSO cardDataLi;
   public int amount;
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        cardDataEntry other = (cardDataEntry)obj;
        return cardDataLi == other.cardDataLi;
    }

    public override int GetHashCode()
    {
        return cardDataLi.GetHashCode();
    }
}
