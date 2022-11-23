using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayState : BaseState
{
    public static Action EnterPlayStateEvent;
    public static Action PauseGameEvent;

    public bool Failed;

    public override void Enter()
    {
        Failed = false;
        Debug.Log("Play State entered");
        EnterPlayStateEvent?.Invoke(); 
    }

    public override void LogicUpdate()
    {
        // Pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGameEvent?.Invoke();
        }
    }
}

