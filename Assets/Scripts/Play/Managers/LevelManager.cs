using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

        SpawnManager.AbducteeWinLoseEvent += NextLevel;
    }

    void OnDisable()
    {
        StartLevelState.EnterStartLevelStateEvent -= SendCurrentLevelEvent;

        SpawnManager.AbducteeWinLoseEvent -= NextLevel;
    }

    void Start()
    {
        if (!MenuData.StoryModeOn)
        {
            _currentLevel = MenuData.LevelSelect;
        }
        else
        {
            //_currentLevel = 0;
        }
    }

    void SendCurrentLevelEvent()
    {
        SendCurrentLevel?.Invoke(_levels[_currentLevel]);
    }

    public void NextLevel(GameObject alien)
    {
        if (alien == null)
        {
            if (_currentLevel < _levels.Length - 1)
            {
                _currentLevel++;
            }
        }
    } 
}
