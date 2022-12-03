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

    [SerializeField]
    private TextMeshProUGUI _remainingText;

    // Used to toggle pause menu
    private bool _toggle;

    void OnEnable()
    {
        // Subscribe to trigger events
        StartLevelState.EnterStartLevelStateEvent += ResetUI;
        PlayState.PauseGameEvent += TogglePauseMenu;

        SpawnManager.ActiveAbducteesEvent += UpdatePlayUI;
        SpawnManager.AbducteeWinLoseEvent += DisplayEndScreen;

        _toggle = false;  
    }

    void OnDisable()
    {
        // Unsubscribe to trigger events
        StartLevelState.EnterStartLevelStateEvent -= ResetUI;
        PlayState.PauseGameEvent -= TogglePauseMenu;

        SpawnManager.ActiveAbducteesEvent -= UpdatePlayUI;
        SpawnManager.AbducteeWinLoseEvent -= DisplayEndScreen;
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

    void UpdatePlayUI(int abducteesRemaining)
    {
        _remainingText.text = "Remaining: " + abducteesRemaining;
    }

    void ResetUI()
    {
        _remainingText.text = "Remaining: 0";
        DisplayMainMenuButton(false);

        foreach (GameObject menu in _menus)
        {
            menu.SetActive(false);
        }
    }

    // Toggles the pause menu on and off
    void TogglePauseMenu()
    {
        _toggle = !_toggle;
        Time.timeScale = _toggle == true ? 0 : 1.0f;
        _menus[(int)MenuType.Pause].SetActive(_toggle);
        DisplayMainMenuButton(_toggle);
        DisplayPlayUI(_toggle);
    }

    void DisplayMainMenuButton(bool toggle)
    {
        _mainMenuButton.SetActive(toggle);
    }

    void DisplayPlayUI(bool toggle)
    {
        _remainingText.enabled = !toggle;
    }

    // UI displayed at EndLevelState
    void DisplayEndScreen(GameObject alien)
    {
        if (alien == null)
        {
            DisplayWinScreen();
        }
        else 
        {
            DisplayLoseScreen();
        }
    }

    void DisplayLoseScreen()
    {
        _menus[(int)MenuType.Lose].SetActive(true);
        DisplayMainMenuButton(true);
    }

    void DisplayWinScreen()
    {
        _menus[(int)MenuType.Win].SetActive(true);
        DisplayMainMenuButton(true);
    }
}
