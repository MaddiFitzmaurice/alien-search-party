using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : BaseState
{
    // Set up a broadcast to indicate to all state machines to change states
    public delegate void PlayStateEvent();
    public delegate void PauseGameEvent();
    public event PlayStateEvent EnterPlayState;
    public event PauseGameEvent PauseGame;

    public bool Failed;

    public override void Enter()
    {
        Failed = false;
        Debug.Log("Play State entered");
        if (EnterPlayState != null)
        {
            EnterPlayState();
        }
    }

    public override void LogicUpdate()
    {
        // Pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame != null)
            {
                PauseGame();
            }
        }
    }
}

