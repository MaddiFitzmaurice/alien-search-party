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
       BeamReset();
       _player.AudioSourceEngine.Play();
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
        BeamReset();
        _player.AudioSourceEngine.Stop();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (_beamActive)
        {
            if (other.CompareTag("Alien"))
            {
                var alien = other.GetComponent<AlienBase>();
                
                if (alien.CanAbduct) 
                {
                    alien.IsUnderBeam = true;
                }
            }
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            var alien = other.GetComponent<AlienBase>();

            if (alien.CanAbduct)
            {
                alien.IsUnderBeam = _beamActive;
            }
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
            _player.AudioSourceBeam.Play();
            _speed = _player.BeamSpeed;
            _player.PlayerAnim.SetFloat("BeamActive", 0.5f);
        }
        else 
        {
            _player.Beam.Stop();
            _player.AudioSourceBeam.Stop();
            _speed = _player.MoveSpeed;
            _player.PlayerAnim.SetFloat("BeamActive", 1f);
        }
    }

    void BeamReset()
    {
        _beamActive = false;
        _player.Beam.Stop();
        _player.AudioSourceBeam.Stop();
        _speed = _player.MoveSpeed;
        _player.PlayerAnim.SetFloat("BeamActive", 1f);
    }
}
