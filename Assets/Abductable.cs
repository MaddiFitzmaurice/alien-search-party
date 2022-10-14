using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abductable : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().CanAbduct = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().CanAbduct = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            other.GetComponent<AlienBase>().CanAbduct = true;
        }
    }
}
