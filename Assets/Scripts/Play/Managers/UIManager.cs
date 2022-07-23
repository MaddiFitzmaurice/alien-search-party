using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MenuType {Pause, Win, Lose};

public class UIManager : MonoBehaviour
{
    public GameObject[] Menus;

    // Used to toggle each menu
    private bool _toggle;

    void Start()
    {
        // Subscribe to trigger events
        GameManager.Instance.PlayState.PauseGame += TogglePauseMenu;
        GameManager.Instance.EndLevelState.EnterEndLevelWinState += ToggleWinMenu;
        GameManager.Instance.EndLevelState.EnterEndLevelLoseState += ToggleLoseMenu;

        _toggle = false;
    }

    void OnDisable()
    {
        // Unsubscribe to trigger events
        GameManager.Instance.PlayState.PauseGame -= TogglePauseMenu;
        GameManager.Instance.EndLevelState.EnterEndLevelWinState -= ToggleWinMenu;
        GameManager.Instance.EndLevelState.EnterEndLevelLoseState -= ToggleLoseMenu;
    }

    // Toggles the pause menu on and off
    void TogglePauseMenu()
    {
        ToggleChange();
        Time.timeScale = _toggle == true ? 0 : 1.0f;
        Menus[(int)MenuType.Pause].SetActive(_toggle);
    }

    void ToggleWinMenu()
    {
        ToggleChange();
        Menus[(int)MenuType.Win].SetActive(_toggle);
    }

    void ToggleLoseMenu()
    {
        ToggleChange();
        Menus[(int)MenuType.Lose].SetActive(_toggle);
    }

    void ToggleChange()
    {
        _toggle = !_toggle;
    }
}
