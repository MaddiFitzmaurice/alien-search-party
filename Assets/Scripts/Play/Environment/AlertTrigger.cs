using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTrigger : MonoBehaviour
{
    private AudioSource _alert;
    
    public void Start()
    {
        _alert = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            _alert.Play();
        }
    }
}
