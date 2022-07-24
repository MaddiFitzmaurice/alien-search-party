using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

enum MenuType {Pause, Win, Lose};

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _menus;

    [SerializeField]
    private GameObject _mainMenuButton;

    // Used to toggle each menu
    private bool _toggle;

    void Start()
    {
        // Subscribe to trigger events
        GameManager.Instance.PlayState.PauseGame += TogglePauseMenu;
        GameManager.Instance.EndLevelState.ShowEndLevelScreen += ToggleEndScreen;

        _toggle = false;
    }

    void OnDisable()
    {
        // Unsubscribe to trigger events
        GameManager.Instance.PlayState.PauseGame -= TogglePauseMenu;
        GameManager.Instance.EndLevelState.ShowEndLevelScreen -= ToggleEndScreen;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    public void StartLevel()
    {
        GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.StartLevelState);
    }

    // Toggles the pause menu on and off
    void TogglePauseMenu()
    {
        ToggleChange();
        Time.timeScale = _toggle == true ? 0 : 1.0f;
        _menus[(int)MenuType.Pause].SetActive(_toggle);
        ToggleMainMenuButton();
    }

    void ToggleEndScreen(int screenDisplay)
    {
        ToggleChange();
        _menus[screenDisplay].SetActive(_toggle);
        ToggleMainMenuButton();
    }

    void ToggleMainMenuButton()
    {
        _mainMenuButton.SetActive(_toggle);
    }

    void ToggleChange()
    {
        _toggle = !_toggle;
    }
}
