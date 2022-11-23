using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndLevelState : BaseState
{
    public static Action EnterEndLevelStateEvent;
    public static Action<int> ShowEndLevelScreenEvent;

    private int _screenDisplayed;

    public override void Enter()
    {
        // Decide whether win or lose screen pops up
        if (GameManager.Instance.PlayState.Failed)
        {
            _screenDisplayed = (int)MenuType.Lose;
        }
        else
        {
            _screenDisplayed = (int)MenuType.Win;
        }

        ShowEndLevelScreenEvent?.Invoke(_screenDisplayed);
        
        EnterEndLevelStateEvent?.Invoke();
        

        Debug.Log("End Level State Entered");
    }

    public override void LogicUpdate()
    {
    }

    public override void Exit()
    {
        
        ShowEndLevelScreenEvent?.Invoke(_screenDisplayed);
        
        Debug.Log("End Level State Left");
    }
}
