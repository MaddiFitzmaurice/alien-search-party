using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MenuOptions {Option1, Option2, Option3};
public enum MenuDisplayed {Main, Story, Select};

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI[]> _menus;
    [SerializeField] private TextMeshProUGUI[] _textMainMenu;
    [SerializeField] private TextMeshProUGUI[] _textStoryMenu;
    [SerializeField] private TextMeshProUGUI[] _textLevelSelect;

    int _menuDisplayed;

    [SerializeField] private GameObject _mainMenuOptions;
    [SerializeField] private GameObject _storyOptions;
    [SerializeField] private GameObject _levelSelectOptions;
    [SerializeField] private GameObject _credits; 

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
    }

    void QuitGame()
    {
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
            MenuData.LevelSelect = selection + 1;
            SceneManager.LoadScene("Play");
        }
        else if (_storyOptions.activeInHierarchy)
        {
            // New Game
            if (selection == (int)MenuOptions.Option3)
            {
                MenuData.LevelStory = 0;
                // Cleared music
            }
            // Load Game
            else if (selection == (int)MenuOptions.Option2)
            {
                MenuData.LoadStoryData();

                if (MenuData.LevelStory == 0)
                {
                    // Fail music
                }
                else
                {
                    //success music
                }
            }
            // Start Game
            else if (selection == (int)MenuOptions.Option1)
            {
                SceneManager.LoadScene("Play");
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

        _mainMenuOptions.SetActive(false);
        _storyOptions.SetActive(true);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(false);

        _menuDisplayed = (int)MenuDisplayed.Story;
    }

    void SelectLevelSelect()
    {
        MenuData.StoryModeOn = false;

        _mainMenuOptions.SetActive(false);
        _storyOptions.SetActive(false);
        _levelSelectOptions.SetActive(true);
        _credits.SetActive(false);

        _menuDisplayed = (int)MenuDisplayed.Select;
    }

    void SelectCredits()
    {
        _mainMenuOptions.SetActive(false);
        _storyOptions.SetActive(false);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(true);
    }

    void BackToMainMenu()
    {
        _mainMenuOptions.SetActive(true);
        _storyOptions.SetActive(false);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(false);

        _menuDisplayed = (int)MenuDisplayed.Main;
    }
}
