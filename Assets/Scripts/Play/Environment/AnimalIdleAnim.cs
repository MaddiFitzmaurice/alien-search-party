using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalIdleAnim : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Random.Range(0, 5) == 4)
        {
            animator.SetTrigger("Eat");
        }
    }
}
