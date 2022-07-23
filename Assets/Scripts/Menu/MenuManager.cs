using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MenuOptions {Option1, Option2, Option3};
public enum MenuDisplayed {Main, Select};

public class MenuManager : MonoBehaviour
{
    private TextMeshProUGUI[][] _textMenuOptions;
    [SerializeField]
    private TextMeshProUGUI[] _textMainMenu;
    [SerializeField]
    private TextMeshProUGUI[] _textLevelSelect;

    int _menuDisplayed;

    [SerializeField]
    private GameObject _mainMenuOptions;
    [SerializeField]
    private GameObject _levelSelectOptions;
    [SerializeField]
    private GameObject _credits; 
    void Start()
    {
        _mainMenuOptions.SetActive(true);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(false);
        _menuDisplayed = (int)MenuDisplayed.Main;

        _textMenuOptions = new TextMeshProUGUI[2][];
        _textMenuOptions[(int)MenuDisplayed.Main] = _textMainMenu;
        _textMenuOptions[(int)MenuDisplayed.Select] = _textLevelSelect; 

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
        _textMenuOptions[_menuDisplayed][num].color = Color.green; 
    }

    void ResetHighlightText(int num)
    {
        _textMenuOptions[_menuDisplayed][num].color = Color.black;
    }

    void ResetAllHighlightText()
    {
        foreach (var menu in _textMenuOptions)
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
        MenuData.LevelSelect = 0;
        SceneManager.LoadScene("Play");
    }

    void SelectLevelSelect()
    {
        _mainMenuOptions.SetActive(false);
        _levelSelectOptions.SetActive(true);
        _credits.SetActive(false);
        _menuDisplayed = (int)MenuDisplayed.Select;
    }

    void SelectCredits()
    {
        _mainMenuOptions.SetActive(false);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(true);
    }

    void BackToMainMenu()
    {
        _mainMenuOptions.SetActive(true);
        _levelSelectOptions.SetActive(false);
        _credits.SetActive(false);
        _menuDisplayed = (int)MenuDisplayed.Main;
    }
}
