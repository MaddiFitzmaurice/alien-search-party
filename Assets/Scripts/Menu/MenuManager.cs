using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MenuOptions {Option1, Option2, Option3};
public enum MenuDisplayed {Main, Story, Select, Credits};
public enum CreditsType {Sound, Art, Thanks}

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI[]> _menus;
    [SerializeField] private TextMeshProUGUI[] _textMainMenu;
    [SerializeField] private TextMeshProUGUI[] _textStoryMenu;
    [SerializeField] private TextMeshProUGUI[] _textLevelSelect;
    [SerializeField] private TextMeshProUGUI[] _textCredits;

    int _menuDisplayed;

    [SerializeField] private GameObject _mainMenuOptions;
    [SerializeField] private GameObject _storyOptions;
    [SerializeField] private GameObject _levelSelectOptions;
    [SerializeField] private GameObject _credits; 
    [SerializeField] private GameObject[] _creditTypes;
    [SerializeField] private GameObject _loadMessage;
    private TextMeshProUGUI _loadText;

    void OnEnable()
    {
        PlayerSelect.BackToMainMenu += BackToMainMenu;
        PlayerSelect.QuitGame += QuitGame;
        PlayerSelect.HoverText += HighlightText;
        PlayerSelect.NoHoverText += ResetHighlightText;
        PlayerSelect.CheckSelection += CheckSelection;
    }

    void OnDisable()
    {   
        PlayerSelect.BackToMainMenu -= BackToMainMenu;
        PlayerSelect.QuitGame -= QuitGame;
        PlayerSelect.HoverText -= HighlightText;
        PlayerSelect.NoHoverText -= ResetHighlightText;
        PlayerSelect.CheckSelection -= CheckSelection;
    }

    void Start()
    {
        _mainMenuOptions.SetActive(true);
        _storyOptions.SetActive(false);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(false);
        _menuDisplayed = (int)MenuDisplayed.Main;

        _menus = new List<TextMeshProUGUI[]>();
        _menus.Add(_textMainMenu);
        _menus.Add(_textStoryMenu);
        _menus.Add(_textLevelSelect);
        _menus.Add(_textCredits);

        _loadText = _loadMessage.GetComponentInChildren<TextMeshProUGUI>();
    }

    void QuitGame()
    {
        StopAllCoroutines();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void HighlightText(int num)
    {
        _menus[_menuDisplayed][num].color = Color.white; 
    }

    void ResetHighlightText(int num)
    {
        _menus[_menuDisplayed][num].color = Color.black;
    }

    void ResetAllHighlightText()
    {
        foreach (var menu in _menus)
        {
            foreach (var text in menu)
            {
                text.color = Color.black;
            }
        }
    }

    void CheckSelection(int selection)
    {
        ResetAllHighlightText();
        
        if (_levelSelectOptions.activeInHierarchy)
        {
            StopAllCoroutines();
            MenuData.LevelSelect = selection + 1;
            SceneManager.LoadScene("Play");
        }
        else if (_storyOptions.activeInHierarchy)
        {
            // New Game
            if (selection == (int)MenuOptions.Option3)
            {
                StopAllCoroutines();
                MenuData.LevelStory = 0;
                SceneManager.LoadScene("Play");
            }
            // Load Game
            else if (selection == (int)MenuOptions.Option2)
            {
                MenuData.LoadStoryData();
                StartCoroutine(LoadMessage());

                if (MenuData.LevelStory == -1)
                {
                    _loadText.text = "No save game found. Press new game to play.";
                }
                else
                {
                    _loadText.text = "Load successful. Press start to play.";
                }
            }
            // Start Game
            else if (selection == (int)MenuOptions.Option1)
            {
                if (MenuData.LevelStory != -1)
                {
                    StopAllCoroutines();
                    SceneManager.LoadScene("Play");
                }
            }
        }
        else if (_credits.activeInHierarchy)
        {
            if (selection == (int)MenuOptions.Option1)
            {
                if (!_creditTypes[(int)MenuOptions.Option1].activeInHierarchy)
                {
                    ShowSoundCredits();
                }
                else
                {
                    HideAllCredits();
                }
            }
            else if (selection == (int)MenuOptions.Option2)
            {
                if (!_creditTypes[(int)MenuOptions.Option2].activeInHierarchy)
                {
                    ShowArtCredits();
                }
                else
                {
                    HideAllCredits();
                }
            }
            else if (selection == (int)MenuOptions.Option3)
            {
                if (!_creditTypes[(int)MenuOptions.Option3].activeInHierarchy)
                {
                    ShowThanksCredits();
                }
                else
                {
                    HideAllCredits();
                }
            }
        }
        else if (_mainMenuOptions.activeInHierarchy)
        {
            switch(selection)
            {
                case 0:
                    SelectStoryMode();
                    break;
                case 1:
                    SelectLevelSelect();
                    break;
                case 2:
                    SelectCredits();
                    break;
                default:
                    return;
            }
        }
    }

    void SelectStoryMode()
    {
        MenuData.StoryModeOn = true;

        HideAllMenus();
        _storyOptions.SetActive(true);

        _menuDisplayed = (int)MenuDisplayed.Story;
    }

    void SelectLevelSelect()
    {
        MenuData.StoryModeOn = false;

        HideAllMenus();
        _levelSelectOptions.SetActive(true);

        _menuDisplayed = (int)MenuDisplayed.Select;
    }

    void SelectCredits()
    {
        HideAllMenus();
        _credits.SetActive(true);

        _menuDisplayed = (int)MenuDisplayed.Credits;
    }

    void HideAllMenus()
    {
        _mainMenuOptions.SetActive(false);
        _storyOptions.SetActive(false);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(false);
    }

    void BackToMainMenu()
    {
        HideAllMenus();
        _mainMenuOptions.SetActive(true);
        ToggleLoadMessage(false);

        _menuDisplayed = (int)MenuDisplayed.Main;
    }

    void ShowSoundCredits()
    {
        HideAllCredits();
        _creditTypes[(int)CreditsType.Sound].SetActive(true);
    }

    void ShowArtCredits()
    {
        HideAllCredits();
        _creditTypes[(int)CreditsType.Art].SetActive(true);
    }

    void ShowThanksCredits()
    {
        HideAllCredits();
        _creditTypes[(int)CreditsType.Thanks].SetActive(true);
    }

    void HideAllCredits()
    {
        foreach (GameObject credits in _creditTypes)
        {
            credits.SetActive(false);
        }
    }

    void ToggleLoadMessage(bool toggle)
    {
        _loadMessage.SetActive(toggle);
    }

    IEnumerator LoadMessage()
    {
        ToggleLoadMessage(true);
        yield return new WaitForSeconds(3);
        ToggleLoadMessage(false);
    }
}
