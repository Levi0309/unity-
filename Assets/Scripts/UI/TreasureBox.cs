using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureBox : MonoBehaviour,IPointerDownHandler
{
    public ObjectEventSO GamewinEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        GamewinEvent.RaisEvent(null, this);
    }

    
}
