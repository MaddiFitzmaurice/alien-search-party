using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float speed;

    private Rigidbody rb;
    private Vector3 vectorSpeed;

    private ParticleSystem beam;

    public float moveSpeed;
    public float beamSpeed;

    public bool beamActive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        beam = GetComponentInChildren<ParticleSystem>();
        beamActive = false;
        speed = moveSpeed;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1")) 
        {
            BeamActivate();
        }
    }

    void FixedUpdate()
    {
        // UFO directional movement
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput).normalized;
        rb.AddForce(move * speed, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (beamActive)
        {
            if (other.CompareTag("Alien"))
            {
                other.GetComponent<AlienBase>().isUnderBeam = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().isUnderBeam = beamActive;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().isUnderBeam = false;
        }
    }

    void BeamActivate()
    {
        beamActive = !beamActive;

        if (beamActive)
        {
            beam.Play();
            speed = beamSpeed;
        }
        else 
        {
            beam.Stop();
            speed = moveSpeed;
        }
    }
}
