using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandle : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private Card currendCard;
    public bool canMove;
    public bool canExcute;
    public GameObject arrowPre;
    private GameObject currentArrow;
    private CharacterBase targetBase;
    private void Awake()
    {
        currendCard = GetComponent<Card>();
       
    }
    public void OnBeginDrag(PointerEventData eventData)
    { if(!currendCard.isAvilable){return;}
        switch (currendCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow= Instantiate(arrowPre, transform.position,Quaternion.identity);
                break;
            case CardType.Deffend:
            case CardType.Abilities:
                canMove=true;
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        canMove=false;
        canExcute=false;
    }
    public void OnDrag(PointerEventData eventData)
    { if(!currendCard.isAvilable){return;}
        if (canMove)
        {
            currendCard.isAnimationing = true;
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currendCard.transform.position = worldPos;
            if (worldPos.y>1)
            {
                canExcute = true;
            }
           
        }
        else 
        {
            if (eventData.pointerEnter == null) return;
            if (eventData.pointerEnter.CompareTag("Enemy"))//卡牌拖到敌人身上 可以执行canExcute方法
            {
                targetBase = eventData.pointerEnter.GetComponent<CharacterBase>();
                canExcute= true;
                return;
            }
            canExcute= false;
            targetBase= null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!currendCard.isAvilable){return;}
        if (currentArrow!=null)
        {
            Destroy(currentArrow);
        }
        if (canExcute)
        {
            currendCard.ExcuteCardEffect(currendCard.player, targetBase);
        }
        else
        {
            currendCard.isAnimationing = false;
            currendCard.ResetPosAndRo();
        }
       
    }

  
}
