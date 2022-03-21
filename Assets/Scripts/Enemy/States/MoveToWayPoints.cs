using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWayPoints : State<EnemyController>
{
	#region Variables
	private Animator animator;
	private CharacterController controller;
	private NavMeshAgent agent;

	protected readonly int hashSlimeMove = Animator.StringToHash("SlimeMove");
	protected readonly int hashMoveSpeed = Animator.StringToHash("MoveSpeed");
	protected readonly int hashSlimeIdle = Animator.StringToHash("SlimeIdle");

	#endregion Variables

	public override void OnInitialized()
	{
		animator = context.GetComponent<Animator>();
		controller = context.GetComponent<CharacterController>();
		agent = context.GetComponent<NavMeshAgent>();
	}

	public override void OnEnter()
	{
		if (context.targetWaypoint == null)
		{
			context.FindNextWayPoint();
		}

		if (context.targetWaypoint)
		{
			agent.SetDestination(context.targetWaypoint.position);
			animator?.SetBool(hashSlimeIdle, false);
			animator?.SetBool(hashSlimeMove, true);
		}
	}

	public override void Update(float deltaTime)
	{
		Transform enemy = context.SearchEnemy();
		if (enemy)
		{
			if (context.IsAvailableAttack)
			{
				stateMachine.ChangeState<AttackState>();
			}
			else
			{
				stateMachine.ChangeState<MoveState>();
			}
		}
		else
		{
			if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
			{
				Transform nextDest = context.FindNextWayPoint();
				if (nextDest)
				{
					agent.SetDestination(nextDest.position);
				}
				stateMachine.ChangeState<IdleState>();
			}
			else
			{
				controller.Move(agent.velocity * deltaTime);
				animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 0.1f, deltaTime);
			}
		}
	}

	public override void OnExit()
	{
		animator?.SetBool(hashSlimeMove, false);
		agent.ResetPath();
	}
}
