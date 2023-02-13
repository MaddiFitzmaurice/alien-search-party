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
    [SerializeField] private List<TextAsset> _cutsceneDialogues;
    [SerializeField] private List<TextAsset> _barkDialogues;
    [SerializeField] private List<AudioClip> _supremeNoises;
    
    [Header("Narrative UI")]
    [SerializeField] private GameObject _panelNarrative;
    [SerializeField] private TextMeshProUGUI _dialogue;
    [SerializeField] private float _dialogueSpeed;
    [SerializeField] private float _speakerNoiseSpeed;
    [SerializeField] private Image _speakerImage;
    [SerializeField] private Sprite[] _alienImages;

    private PlayableDirector _director;
    private AudioSource _audioSource;
    private Level _currentLevel;
    private Story _currentCutsceneDialogue;
    private Story _currentBarkDialogue;
    private PlayableAsset _currentCutscene;
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
        }
    }

    void OnDisable()
    {
        if (MenuData.StoryModeOn)
        {
            LevelManager.SendCurrentLevel -= ChangeNarrativeAssets;

            CutsceneState.EnterCutsceneStateEvent -= PlayCutscene;
            CutsceneState.ExitCutsceneStateEvent -= StopCutscene;
        }
    }

    void ChangeNarrativeAssets(Level level)
    {
        if (level != null)
        {
            _currentLevel = level;
            _currentCutsceneDialogue = new Story(_cutsceneDialogues[level.LevelNumber].text);
            _currentCutscene = _cutscenes[level.LevelNumber];
        }
    }

    public void PlayCutscene()
    {
        _audioSource.enabled = true;
        _panelNarrative.SetActive(true);
        _director.Play(_currentCutscene);
    }

    public void StopCutscene()
    {
        _audioSource.enabled = false;
        StopAllCoroutines();
        _director.time = _director.duration;
        _director.Evaluate();
        _director.Stop();
        _panelNarrative.SetActive(false);

        // If final level
        if (_currentLevel.AmountTotal == 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void NextDialogue()
    {
        if (_currentCutsceneDialogue.canContinue)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            string line = _currentCutsceneDialogue.Continue();
            string speaker = HandleTag(_currentCutsceneDialogue.currentTags);
            SetSpeaker(speaker);
            _currentCoroutine = StartCoroutine(TypingEffect(line));
        }
        else
        {
            _dialogue.text = "";
        }
    }

    private IEnumerator TypingEffect(string line)
    {
        _dialogue.text = "";
        int visibleChars = 0;

        foreach (char letter in line.ToCharArray())
        {
            _dialogue.text += letter;
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
