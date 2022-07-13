using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private float _speed;

    private Rigidbody _rb;
    private Vector3 _vectorSpeed;

    private ParticleSystem _beam;

    public float MoveSpeed;
    public float BeamSpeed;

    public bool BeamActive;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _beam = GetComponentInChildren<ParticleSystem>();
        BeamActive = false;
        _speed = MoveSpeed;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1")) 
        {
            BeamActivate();
        }
    }

    void FixedUpdate()
    {
        // UFO directional movement
        Vector3 move = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
        _rb.AddForce(move * _speed, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (BeamActive)
        {
            if (other.CompareTag("Alien"))
            {
                other.GetComponent<AlienBase>().IsUnderBeam = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().IsUnderBeam = BeamActive;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().IsUnderBeam = false;
        }
    }

    void BeamActivate()
    {
        BeamActive = !BeamActive;

        if (BeamActive)
        {
            _beam.Play();
            _speed = BeamSpeed;
        }
        else 
        {
            _beam.Stop();
            _speed = MoveSpeed;
        }
    }
}
