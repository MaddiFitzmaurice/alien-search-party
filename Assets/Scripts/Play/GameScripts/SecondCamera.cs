using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamera : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private Vector3 _camOffset;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        StartLevelState.EnterStartLevelStateEvent += HideAlienCamera;
        AlienBase.AlienReachedDestEvent += ShowAlienCamera;
    }

    void OnDisable()
    {
        StartLevelState.EnterStartLevelStateEvent -= HideAlienCamera;
        AlienBase.AlienReachedDestEvent -= ShowAlienCamera;
    }

    void ShowAlienCamera(GameObject alien)
    {
        transform.position = alien.transform.position + _camOffset;
        _camera.enabled = true;
    }

    void HideAlienCamera()
    {
        _camera.enabled = false;
    }
}
