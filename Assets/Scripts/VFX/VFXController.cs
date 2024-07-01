using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject Buff;
    public GameObject DeBuff;
    public float delayTime;
    private float timeCounter;

    private void Update()
    {
        if(Buff.activeInHierarchy)
        {
            timeCounter+=Time.deltaTime;
            if(timeCounter>delayTime)
            {
                timeCounter=0;
                Buff.SetActive(false);

            }

        }
        if(DeBuff.activeInHierarchy)
        {
            timeCounter+=Time.deltaTime;
            if(timeCounter>delayTime)
            {
                timeCounter=0;
                DeBuff.SetActive(false);

            }

        }
        
    }
}
