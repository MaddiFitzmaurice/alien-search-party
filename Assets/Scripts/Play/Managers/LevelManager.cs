using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    // Events
    public static Action<Level> SendCurrentLevel;

    // Fields
    [SerializeField]
    private Level[] _levels;
    [SerializeField]
    private int _currentLevel;

    void OnEnable()
    {
        StartLevelState.EnterStartLevelStateEvent += SendCurrentLevelEvent;

        SpawnManager.AllAliensCaughtEvent += NextLevel;
    }

    void OnDisable()
    {
        StartLevelState.EnterStartLevelStateEvent -= SendCurrentLevelEvent;

        SpawnManager.AllAliensCaughtEvent -= NextLevel;
    }

    void Start()
    {
        /*if (!MenuData.StoryModeOn)
        {
            _currentLevel = MenuData.LevelSelect;
        }
        else
        {
            _currentLevel = 0;
        }*/
    }

    void SendCurrentLevelEvent()
    {
        SendCurrentLevel?.Invoke(_levels[_currentLevel]);
    }

    // EndLevelState will call this function if needed
    public void NextLevel()
    {
        if (_currentLevel == _levels.Length - 1)
        {
            // End game and back to menu logic
        }
        else
        {
            _currentLevel++;
        }
    } 
}
