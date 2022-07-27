using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAlien : MonoBehaviour
{
    public Animator MenuAlienAnim;
    public void Abduct()
    {
        MenuAlienAnim.SetBool("Abduct", true);
    }

    public void Idle()
    {
        MenuAlienAnim.SetBool("Abduct", false);
    }
}
