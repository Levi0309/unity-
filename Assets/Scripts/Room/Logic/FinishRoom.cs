using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRoom : MonoBehaviour
{
    public ObjectEventSO LoadMapEvent;
    private void OnMouseDown()
    {
        Debug.Log("╪сть");
        LoadMapEvent.RaisEvent(null,this);
    }
}
