using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlState : BaseState
{
    private Player _player; 

    // Input Variables
    private float _horizontalInput;
    private float _verticalInput;

    // Speed Variables
    private float _speed;
    private Vector3 _vectorSpeed;

    // Beam
    private bool _beamActive;

    public PlayerControlState(Player player)
    {
        _player = player;
    }

     public override void Enter()
    {
        _beamActive = false;
        _speed = _player.MoveSpeed;
    }

    public override void LogicUpdate()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1")) 
        {
            BeamActivate();
        }
    }

    public override void PhysicsUpdate()
    {
        // UFO directional movement
        Vector3 move = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
        _player.Rb.AddForce(move * _speed, ForceMode.Acceleration);
    }

    public override void Exit()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (_beamActive)
        {
            if (other.CompareTag("Alien"))
            {
                other.GetComponent<AlienBase>().IsUnderBeam = true;
            }
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().IsUnderBeam = _beamActive;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().IsUnderBeam = false;
        }
    }

    void BeamActivate()
    {
        _beamActive = !_beamActive;

        if (_beamActive)
        {
            _player.Beam.Play();
            _speed = _player.BeamSpeed;
        }
        else 
        {
            _player.Beam.Stop();
            _speed = _player.MoveSpeed;
        }
    }
}
