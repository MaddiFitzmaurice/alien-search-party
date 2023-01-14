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
    // BeamTrigger normal y
    private Vector3 _beamNormalY = new Vector3(0, -1, 0);
    // BeamTrigger reduced y
    private Vector3 _beamReducedY = Vector3.zero;
    private bool _soundMute = false;

    public PlayerControlState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        _soundMute = false;
        AudioListener.pause = _soundMute;
        Abductee.AbductEvent += PlayAbductSound;
        PlayState.PauseGameEvent += MuteSound;
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
        Abductee.AbductEvent -= PlayAbductSound;
        PlayState.PauseGameEvent -= MuteSound;
        BeamReset();
        _player.AudioSourceEngine.Stop();
    }

    public void BeamActivate()
    {
        _beamActive = !_beamActive;

        if (_beamActive)
        {
            _player.BeamTrigger.center = _beamNormalY;
            _player.Beam.Play();
            _player.AudioSourceBeam.Play();
            _speed = _player.BeamSpeed;
            _player.PlayerAnim.SetFloat("BeamActive", 0.5f);
        }
        else 
        {
           BeamReset();
        }
    }

    public void BeamReset()
    {
        _beamActive = false;
        _player.BeamTrigger.center = _beamReducedY;
        _player.Beam.Stop();
        _player.AudioSourceBeam.Stop();
        _speed = _player.MoveSpeed;
        _player.PlayerAnim.SetFloat("BeamActive", 1f);
    }

    public void PlayAbductSound()
    {
        _player.AudioSourceAbduct.Play();
    }

    public void MuteSound()
    {
        _soundMute = !_soundMute;
        AudioListener.pause = _soundMute;
    }
}
