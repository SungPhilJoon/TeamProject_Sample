using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController>
{
	#region Variables
	private Animator animator;

	protected int hashIsSlimeAlive = Animator.StringToHash("IsSlimeAlive");

    #endregion Variables

    public override void OnInitialized()
	{
		animator = context.GetComponent<Animator>();
		animator?.SetBool(hashIsSlimeAlive, true);
	}

	public override void OnEnter()
	{
		animator?.SetBool(hashIsSlimeAlive, false);
	}

	public override void Update(float deltaTime)
	{
		if (stateMachine.ElapsedTimeInState > 3.0f)
		{
			GameObject.Destroy(context.gameObject);
		}
	}

	public override void OnExit()
	{
		
	}
}
