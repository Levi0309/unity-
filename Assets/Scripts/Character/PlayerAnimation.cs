using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{  private Player player;
    private Animator anim;

    private void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        anim.Play("sleep");
        anim.SetBool("isSleep", true);
    }

    public void PlayerTurnBeginAnimation()
    {
        anim.SetBool("isSleep", false);
        anim.SetBool("isParry", false);
    }

    public void PlayerTurnEndAnimation()
    {
        if (player.Defence.currentValue > 0)
        {
            anim.SetBool("isParry", true);
        }
        else
        {
            anim.SetBool("isSleep", true);
            anim.SetBool("isParry", false);
        }
    }

   public void DiscardEvent(object obj)
   {    
        Card card=obj as Card;
        switch(card.cardData.cardType)
        {
            case CardType.Attack:
                anim.SetTrigger("attack");
            break;
            case CardType.Deffend:
            break;
            case CardType.Abilities:
                anim.SetTrigger("skill");
            break;

        }
   }
    public void PlayAnimSleep() 
    {
        anim.Play("death");
    }

}
