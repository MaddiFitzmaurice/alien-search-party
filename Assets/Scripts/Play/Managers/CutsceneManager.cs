using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private PlayableAsset[] _cutscenes;
    private PlayableDirector _playableDirector;
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playableDirector.playableAsset = _cutscenes[0];
        _playableDirector.Play(_cutscenes[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
