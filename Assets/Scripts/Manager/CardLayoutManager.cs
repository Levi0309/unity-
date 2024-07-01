using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    [Header("?????")]
    public float maxWidth = 7f;
    public float Cardspacing = 2f;
    [Header("???¦Â???")]
    public float radius = 17f;
    public float angleSpacing = 7f;
    public List<Vector3> Cardspostion = new();
    public List<Quaternion> CardsRotation = new();
    public Vector3 centerPoint;


    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -3.9f : Vector3.up * -20.8f;


    }
    public CardTransform GetTransform(int index,int maxCards) 
    {
        CalculatePosition(maxCards, isHorizontal);
        return new CardTransform(Cardspostion[index], CardsRotation[index]);
    }

    private void CalculatePosition(int maxCardsCount,bool horizontal) 
    {
        Cardspostion.Clear();
        CardsRotation.Clear();
        if (horizontal)
        {
            float currentWidth = Cardspacing * (maxCardsCount - 1);//??????§á????????????? 2*3=6 ???????????x?????????????x? ????????????
            float totalWidth = Mathf.Min(currentWidth, maxWidth);
            float currentSpacing = totalWidth > 0 ? totalWidth / (maxCardsCount - 1) : 0;//????????? ?????????????????????? ???????§³??
            for (int i = 0; i < maxCardsCount; i++)
            {
               
                float xPos=0-(totalWidth/2)+i*currentSpacing;
                var Cardpos=new Vector3(xPos, centerPoint.y, 0);
                var rotation = Quaternion.identity;
                Cardspostion.Add(Cardpos);
                CardsRotation.Add(rotation);

            }
        }
        else 
        {
            float currentAngle = angleSpacing * (maxCardsCount - 1)/2;//???????? ?????????2???currentAngle - i * angleSpacing??????????????0???
            for (int i = 0; i < maxCardsCount; i++)
            {
                var Cardpos = FanCardPos(currentAngle - i * angleSpacing);
              
                var rotation = Quaternion.Euler(0, 0, currentAngle - i * angleSpacing);
                Cardspostion.Add(Cardpos);
               
                CardsRotation.Add(rotation);
            }
            
         

        }


    }
    /// <summary>
    /// ????????????¦Ï???¦Ë??
    /// </summary>
    /// <returns></returns>
    private Vector3 FanCardPos(float angle) 
    {
        return new Vector3(centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius, centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius);


    }
}
