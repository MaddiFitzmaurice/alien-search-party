using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class NarrativeManager : MonoBehaviour
{
    [Header("Narrative Assets")]
    [SerializeField] private List<PlayableAsset> _cutscenes;
    [SerializeField] private List<PlayableAsset> _barks;
    [SerializeField] private List<TextAsset> _cutsceneDialogues;
    [SerializeField] private List<TextAsset> _barkDialogues;
    [SerializeField] private List<AudioClip> _supremeNoises;
    
    [Header("Narrative UI")]
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private float _dialogueSpeed;
    [SerializeField] private float _speakerNoiseSpeed;
    [SerializeField] private Image _speakerImage;
    [SerializeField] private Sprite[] _alienImages;

    private PlayableDirector _director;
    private AudioSource _audioSource;
    private Level _currentLevel;

    private Story _currentDialogue;
    private Story _currentCutsceneDialogue;
    private Story _currentBarkDialogue;
    private PlayableAsset _currentCutscene;
    private PlayableAsset _currentBark;
    private Coroutine _currentCoroutine;

    void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (MenuData.StoryModeOn)
        {
            LevelManager.SendCurrentLevel += ChangeNarrativeAssets;

            CutsceneState.EnterCutsceneStateEvent += PlayCutscene;
            CutsceneState.ExitCutsceneStateEvent += StopCutscene;

            BarkState.EnterBarkStateEvent += PlayBark;
            BarkState.ExitBarkStateEvent += StopBark;
        }
    }

    void OnDisable()
    {
        if (MenuData.StoryModeOn)
        {
            LevelManager.SendCurrentLevel -= ChangeNarrativeAssets;

            CutsceneState.EnterCutsceneStateEvent -= PlayCutscene;
            CutsceneState.ExitCutsceneStateEvent -= StopCutscene;

            BarkState.EnterBarkStateEvent -= PlayBark;
            BarkState.ExitBarkStateEvent -= StopBark;
        }
    }

    void ChangeNarrativeAssets(Level level)
    {
        if (level != null)
        {
            _currentLevel = level;

            _currentCutsceneDialogue = new Story(_cutsceneDialogues[level.LevelNumber].text);
            _currentCutscene = _cutscenes[level.LevelNumber];

            // Account for no bark levels (tutorial and final)
            if (level.LevelNumber != 0 && level.AmountTotal != 0)
            {
                _currentBarkDialogue = new Story(_barkDialogues[level.LevelNumber - 1].text);
                _currentBark = _barks[level.LevelNumber - 1];
            }
        }
    }

    public void PlayCutscene()
    {
        _currentDialogue = _currentCutsceneDialogue;
        _audioSource.enabled = true;
        _director.Play(_currentCutscene);
    }

    public void StopCutscene()
    {
        _audioSource.enabled = false;
        StopAllCoroutines();
        _director.time = _director.duration;
        _director.Evaluate();
        _director.Stop();

        // If final level
        if (_currentLevel.AmountTotal == 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void PlayBark()
    {
        _audioSource.enabled = true;
        _currentDialogue = _currentBarkDialogue;
        _director.Play(_currentBark);
    }

    public void StopBark()
    {
        _audioSource.enabled = false;
        StopAllCoroutines();
    }

    public void ExitTimeline()
    {
        _director.time = _director.duration;
        _director.Evaluate();
        _director.Stop();
        GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.PlayState);  
    }

    public void NextDialogue()
    {
        if (_currentDialogue.canContinue)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            string line = _currentDialogue.Continue();
            string speaker = HandleTag(_currentDialogue.currentTags);
            SetSpeaker(speaker);
            _currentCoroutine = StartCoroutine(TypingEffect(line));
        }
        else
        {
            _dialogueText.text = "";
        }
    }

    private IEnumerator TypingEffect(string line)
    {
        _dialogueText.text = "";
        int visibleChars = 0;

        foreach (char letter in line.ToCharArray())
        {
            _dialogueText.text += letter;
            PlayTalkingAudio(visibleChars, letter);
            visibleChars++;
            yield return new WaitForSeconds(_dialogueSpeed);
        }
    } 

    void PlayTalkingAudio(int chars, char letter)
    {
        if (chars % _speakerNoiseSpeed == 0)
        {
            int noiseSelection = letter.GetHashCode() % _supremeNoises.Count;
            _audioSource.PlayOneShot(_supremeNoises[noiseSelection]);
        }
    }

    void SetSpeaker(string speaker)
    {
        if (speaker == "green")
        {
            _speakerImage.sprite = _alienImages[0];
            _audioSource.pitch = 1f;
        }
        else if (speaker == "grey")
        {
            _speakerImage.sprite = _alienImages[1];
            _audioSource.pitch = 0.7f;
        }
    }
    
    public string HandleTag(List<string> currentTags)
    {
        if (currentTags.Count != 0)
        {
            string speaker = currentTags[0].Trim();

            if (speaker == "green")
            {
                return "green";
            }
            else if (speaker == "grey")
            {
                return "grey";
            }
        }

        return null;
    }
}
