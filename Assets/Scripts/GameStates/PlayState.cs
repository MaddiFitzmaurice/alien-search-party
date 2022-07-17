using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : BaseState
{
    // Set up a broadcast to indicate to all state machines to change states
    public delegate void PlayStateEvent();
    public event PlayStateEvent EnterPlayState; 

    public override void Enter()
    {
        Debug.Log("Play State entered");
        if (EnterPlayState != null)
        {
            EnterPlayState();
        }
    }
}

