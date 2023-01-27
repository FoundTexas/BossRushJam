using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDirUntilRay : StateMachineBehaviour
{
    public float Length = 1, speed = 1;
    public string nextState = "Started";
    public LayerMask hitLayer;
    public Vector2 dir;
    Transform transform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!Physics2D.Raycast(transform.position, dir, Length, hitLayer))
            transform.Translate(dir*speed*Time.deltaTime);
        else if(Physics2D.Raycast(transform.position, dir, Length, hitLayer))
            animator.SetTrigger(nextState);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
