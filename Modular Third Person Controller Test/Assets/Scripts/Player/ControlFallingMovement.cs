using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFallingMovement : StateMachineBehaviour
{
    private void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerMovement>().HasPlayerControl = false;
    }
    private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent <PlayerMovement>().HasPlayerControl = true;
    }
}
