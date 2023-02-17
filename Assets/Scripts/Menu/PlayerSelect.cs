using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSelect : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private Rigidbody _Rb;
    private CapsuleCollider _beamCollider;
    private AudioSource _audioSource;
    private int _currentSelection;

    public static Action BackToMainMenu;
    public static Action QuitGame;
    public static Action<int> CheckSelection;
    public static Action<int> NoHoverText;
    public static Action<int> HoverText;

    [SerializeField] private float _speed;
    void Start()
    {
        _Rb = GetComponent<Rigidbody>();
        _beamCollider = GetComponent<CapsuleCollider>();
        _audioSource = GetComponent<AudioSource>();
        _currentSelection = -1;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BackToMainMenu?.Invoke();

            // If UFO is hovering over a selection, make sure to highlight new selection
            if (_currentSelection != -1)
            {
                HoverText?.Invoke(_currentSelection);
            }
        }

        if (Input.GetButtonDown("Fire1")) 
        {
            _beamCollider.enabled = false;

            if (_currentSelection != -1)
            {
                CheckSelection?.Invoke(_currentSelection);
                _audioSource.Play();
            }
            
            _beamCollider.enabled = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(_horizontalInput, 0, 0).normalized;
        _Rb.AddForce(move * _speed, ForceMode.Acceleration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Option 1")
        {
            _currentSelection = (int)MenuOptions.Option1;
            
            HoverText?.Invoke((int)MenuOptions.Option1);
            other.GetComponentInChildren<Animator>().SetBool("Abduct", true);
        }
        else if (other.tag == "Option 2")
        {
            _currentSelection = (int)MenuOptions.Option2;
            
            HoverText?.Invoke((int)MenuOptions.Option2);
            other.GetComponentInChildren<Animator>().SetBool("Abduct", true);
        }
        else if (other.tag == "Option 3")
        {
            _currentSelection = (int)MenuOptions.Option3;
           
            HoverText?.Invoke((int)MenuOptions.Option3);
            other.GetComponentInChildren<Animator>().SetBool("Abduct", true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Option 1")
        {
            _currentSelection = -1;
            
            NoHoverText?.Invoke((int)MenuOptions.Option1);
            other.GetComponentInChildren<Animator>().SetBool("Abduct", false);
        }
        else if (other.tag == "Option 2")
        {
            _currentSelection = -1;
            
            NoHoverText?.Invoke((int)MenuOptions.Option2);
            other.GetComponentInChildren<Animator>().SetBool("Abduct", false);
        }
        else if (other.tag == "Option 3")
        {
            _currentSelection = -1;
           
            NoHoverText?.Invoke((int)MenuOptions.Option3);
            other.GetComponentInChildren<Animator>().SetBool("Abduct", false);
        }
    }
}
