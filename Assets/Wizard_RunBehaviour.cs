using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_RunBehaviour : StateMachineBehaviour
{
    private Enemy enemy;

    private Rigidbody2D rb2D;

    [SerializeField]
    private float moveSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("RandomAttack", Random.Range(0, 2));
        enemy = animator.GetComponent<Enemy>();
        rb2D = enemy.rb2D;

        enemy.SeePlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y) * animator.transform.right;
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(enemy.playerDistanceX <= 2)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
       
    }

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
