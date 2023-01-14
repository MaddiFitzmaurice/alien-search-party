using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamera : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private Vector3 _camOffset;

    [SerializeField]
    private Vector3 _camAngle;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        StartLevelState.EnterStartLevelStateEvent += HideAlienCamera;
        Alien.AlienReachedDestEvent += ShowAlienCamera;
    }

    void OnDisable()
    {
        StartLevelState.EnterStartLevelStateEvent -= HideAlienCamera;
        Alien.AlienReachedDestEvent -= ShowAlienCamera;
    }

    void ShowAlienCamera(GameObject alien)
    {
        transform.position = alien.transform.position + _camOffset;
        transform.eulerAngles = _camAngle;
        _camera.enabled = true;
    }

    void HideAlienCamera()
    {
        _camera.enabled = false;
    }
}
