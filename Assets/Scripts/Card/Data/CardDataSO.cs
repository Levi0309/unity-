using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="CardDataSO",menuName ="Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public int cardCost;
    public Sprite cardImage;
    [TextArea] 
    public string cardDecription;
    public List<CardEffect> cardEffects;
}
