using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomStateMachineBehaviour : StateMachineBehaviour
{
	#region Variables
	public int numberOfStates = 3;
	public float minNormTime = 2.0f;
	public float maxNormTime = 5.0f;

	public float randomNormalTime;

	readonly int hashIdleIndex = Animator.StringToHash("IdleIndex");

	#endregion Variables
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		randomNormalTime = Random.Range(minNormTime, maxNormTime);
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If transitioning away from this state reset the random idle parameter to -1
		if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
		{
			animator.SetInteger(hashIdleIndex, -1);
		}

		// If the state is beyond the randomly decide normalised time and not yet transitioning
		if (stateInfo.normalizedTime > randomNormalTime && !animator.IsInTransition(0))
		{
			animator.SetInteger(hashIdleIndex, Random.Range(1, numberOfStates));
		}
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
