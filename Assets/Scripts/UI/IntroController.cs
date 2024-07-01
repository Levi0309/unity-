using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    private PlayableDirector playable;
    public ObjectEventSO LoadMenuEvent;

    private void Awake()
    {
        playable= GetComponent<PlayableDirector>();
        playable.stopped += OnPlayableDirectorStopped;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&playable.state==PlayState.Playing)
        {
            playable.Stop();
           
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector obj)
    {
        LoadMenuEvent.RaisEvent(null, this);
    }
}
