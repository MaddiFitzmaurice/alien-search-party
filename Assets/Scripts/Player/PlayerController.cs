using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    private Rigidbody rb;
    private Vector3 vectorSpeed;

    public float speed;
    public float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vectorSpeed = new Vector3(0, spinSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // UFO directional movement
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput).normalized;
        
        rb.AddForce(move * speed, ForceMode.Acceleration);

        // UFO spin
        Quaternion deltaRotation = Quaternion.Euler(vectorSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
