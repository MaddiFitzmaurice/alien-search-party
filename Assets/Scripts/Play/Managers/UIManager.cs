using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

enum MenuType {Pause, Win, Lose};
enum PanelType {Menu, Play, Narrative};

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _panels;
    [SerializeField]
    private GameObject[] _menus;

    [SerializeField]
    private TextMeshProUGUI _textDetected;

    // Used to toggle UI
    private bool _toggle;

    void OnEnable()
    {
        // Subscribe to trigger events
        StartLevelState.EnterStartLevelStateEvent += ResetUI;

        PlayState.EnterPlayStateEvent += EnterPlayStateEventHandler;
        PlayState.PauseGameEvent += ToggleMenuPause;

        SpawnManager.ActiveAbducteesEvent += UpdatePlayPanel;
        SpawnManager.AbducteeWinLoseEvent += DisplayEndScreen;

        if (MenuData.StoryModeOn)
        {
            CutsceneState.EnterCutsceneStateEvent += DisplayNarrativePanel;
            CutsceneState.ExitCutsceneStateEvent += HideNarrativePanel;

            BarkState.EnterBarkStateEvent += DisplayNarrativePanel;
            BarkState.ExitBarkStateEvent += HideNarrativePanel;
        }

        _toggle = false;  
    }

    void OnDisable()
    {
        // Unsubscribe to trigger events
        StartLevelState.EnterStartLevelStateEvent -= ResetUI;

        PlayState.EnterPlayStateEvent -= EnterPlayStateEventHandler;
        PlayState.PauseGameEvent -= ToggleMenuPause;

        SpawnManager.ActiveAbducteesEvent -= UpdatePlayPanel;
        SpawnManager.AbducteeWinLoseEvent -= DisplayEndScreen;

        if (MenuData.StoryModeOn)
        {
            CutsceneState.EnterCutsceneStateEvent -= DisplayNarrativePanel;
            CutsceneState.ExitCutsceneStateEvent -= HideNarrativePanel;

            BarkState.EnterBarkStateEvent -= DisplayNarrativePanel;
            BarkState.ExitBarkStateEvent -= HideNarrativePanel;
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SceneManager.LoadScene("Menu");
    }

    public void StartLevel()
    {
        GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.StartLevelState);
    }

    void UpdatePlayPanel(int abducteesRemaining)
    {
        _textDetected.text = abducteesRemaining.ToString();
    }

    // Resets all UI elements at start of game
    void ResetUI()
    {
        _textDetected.text = "0";

        foreach (GameObject panel in _panels)
        {
            panel.SetActive(false);
        }

        foreach (GameObject menu in _menus)
        {
            menu.SetActive(false);
        }
    }

    // Handles EnterPlayState event
    void EnterPlayStateEventHandler()
    {
        TogglePanelPlay(true);
    }

    void TogglePanelMenu(bool toggle)
    {
        _panels[(int)PanelType.Menu].SetActive(toggle);
    }

    void TogglePanelPlay(bool toggle)
    {
        _panels[(int)PanelType.Play].gameObject.SetActive(toggle);
    }

    void TogglePanelNarrative(bool toggle)
    {
        _panels[(int)PanelType.Narrative].gameObject.SetActive(toggle);
    }
    void HideNarrativePanel()
    {
        TogglePanelNarrative(false);
        TogglePanelPlay(true);
    }

    void DisplayNarrativePanel()
    {
        TogglePanelNarrative(true);
        TogglePanelPlay(false);
    }

    // Toggles the pause menu on and off
    void ToggleMenuPause()
    {
        _toggle = !_toggle;
        Time.timeScale = _toggle == true ? 0 : 1.0f;

        TogglePanelMenu(_toggle);
        TogglePanelPlay(!_toggle);

        _menus[(int)MenuType.Pause].SetActive(_toggle);
    }

    // UI displayed at EndLevelState
    void DisplayEndScreen(GameObject alien)
    {
        TogglePanelMenu(true);

        // Win screen
        if (alien == null)
        {
            _menus[(int)MenuType.Win].SetActive(true);
        }
        // Lose screen
        else 
        {
            _menus[(int)MenuType.Lose].SetActive(true);
        }
    }
}
