using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelState : BaseState
{
    public delegate void EndLevelStateEvent();
    public delegate void EndLevelStateOutcomeEvent(int num);
    public event EndLevelStateEvent EnterEndLevelState;
    public event EndLevelStateOutcomeEvent ShowEndLevelScreen;

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

        if (ShowEndLevelScreen != null)
        {
            ShowEndLevelScreen(_screenDisplayed);
        }

        if (EnterEndLevelState != null)
        {
            EnterEndLevelState();
        }

        Debug.Log("End Level State Entered");
    }

    public override void LogicUpdate()
    {
    }

    public override void Exit()
    {
        if (ShowEndLevelScreen != null)
        {
            ShowEndLevelScreen(_screenDisplayed);
        }
        Debug.Log("End Level State Left");
    }
}
