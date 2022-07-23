using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private Rigidbody _Rb;
    private CapsuleCollider _beamCollider;
    private int _currentSelection;

    public delegate void PlayerSelectEvent();
    public delegate void PlayerConfirmSelectionEvent(int num);
    public delegate void PlayerHoverEvent(int num);
    public static event PlayerHoverEvent HoverText;
    public static event PlayerHoverEvent NoHoverText;
    public static event PlayerConfirmSelectionEvent CheckSelection;
    public static event PlayerSelectEvent BackToMainMenu;
    public static event PlayerSelectEvent QuitGame;

    public float Speed;
    void Start()
    {
        _Rb = GetComponent<Rigidbody>();
        _beamCollider = GetComponent<CapsuleCollider>();
        _currentSelection = -1;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (QuitGame != null)
            {
                QuitGame();
            }   
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (BackToMainMenu != null)
            {
                BackToMainMenu();
            }
        }

        if (Input.GetButtonDown("Fire1")) 
        {
            _beamCollider.enabled = false;
            if (CheckSelection != null)
            {
                if (_currentSelection != -1)
                {
                    CheckSelection(_currentSelection);
                }
            }
            _beamCollider.enabled = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(_horizontalInput, 0, 0).normalized;
        _Rb.AddForce(move * Speed, ForceMode.Acceleration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Option 1")
        {
            _currentSelection = (int)MenuOptions.Option1;
            if (HoverText != null)
            {
                HoverText((int)MenuOptions.Option1);
            }
        }
        else if (other.tag == "Option 2")
        {
            _currentSelection = (int)MenuOptions.Option2;
            if (HoverText != null)
            {
                HoverText((int)MenuOptions.Option2);
            }
        }
        else if (other.tag == "Option 3")
        {
            _currentSelection = (int)MenuOptions.Option3;
            if (HoverText != null)
            {
                HoverText((int)MenuOptions.Option3);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Option 1")
        {
            _currentSelection = -1;
            if (NoHoverText != null)
            {
                NoHoverText((int)MenuOptions.Option1);
            }
        }
        else if (other.tag == "Option 2")
        {
            _currentSelection = -1;
            if (NoHoverText != null)
            {
                NoHoverText((int)MenuOptions.Option2);
            }
        }
        else if (other.tag == "Option 3")
        {
            _currentSelection = -1;
            if (NoHoverText != null)
            {
                NoHoverText((int)MenuOptions.Option3);
            }
        }
    }
}
