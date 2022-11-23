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
    private TextMeshProUGUI _aliensRemainingText;

    // Used to toggle each menu
    private bool _toggle;

    void OnEnable()
    {
        // Subscribe to trigger events
        StartLevelState.EnterStartLevelStateEvent += ResetPlayUI;
        PlayState.PauseGameEvent += TogglePauseMenu;
        EndLevelState.ShowEndLevelScreenEvent += ToggleEndScreen;

        AlienBase.AlienSpawnEvent += UpdatePlayUI;
        AlienBase.AlienAbductEvent += UpdatePlayUI;
        
        _toggle = false;
    }

    void OnDisable()
    {
        // Unsubscribe to trigger events
        StartLevelState.EnterStartLevelStateEvent -= ResetPlayUI;
        PlayState.PauseGameEvent -= TogglePauseMenu;
        EndLevelState.ShowEndLevelScreenEvent -= ToggleEndScreen;

        AlienBase.AlienSpawnEvent -= UpdatePlayUI;
        AlienBase.AlienAbductEvent -= UpdatePlayUI;
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

    void UpdatePlayUI(int aliensRemaining)
    {
        Debug.Log(aliensRemaining);
        _aliensRemainingText.text = "Aliens Remaining: " + aliensRemaining;
    }

    void ResetPlayUI()
    {
        _aliensRemainingText.text = "Aliens Remaining: 0";
    }

    // Toggles the pause menu on and off
    void TogglePauseMenu()
    {
        ToggleChange();
        Time.timeScale = _toggle == true ? 0 : 1.0f;
        _menus[(int)MenuType.Pause].SetActive(_toggle);
        ToggleMainMenuButton();
        TogglePlayUI();
    }

    void TogglePlayUI()
    {
        _aliensRemainingText.enabled = !_toggle;
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
