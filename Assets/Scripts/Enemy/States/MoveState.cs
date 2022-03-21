using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<EnemyController>
{
	private Animator animator;
	private CharacterController controller;
	private NavMeshAgent agent;

	private readonly int hashSlimeIdle = Animator.StringToHash("SlimeIdle");
	private readonly int hashSlimeMove = Animator.StringToHash("SlimeMove");
	private readonly int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

	public override void OnInitialized()
	{
		animator = context.GetComponent<Animator>();
		controller = context.GetComponent<CharacterController>();
		agent = context.GetComponent<NavMeshAgent>();
	}

	public override void OnEnter()
	{
		agent?.SetDestination(context.Target.position);
		animator?.SetBool(hashSlimeIdle, false);
		animator?.SetBool(hashSlimeMove, true);
	}

	public override void Update(float deltaTime)
	{
		Transform enemy = context.SearchEnemy();
		if (enemy)
		{
			agent.SetDestination(context.Target.position);

			if (agent.remainingDistance > agent.stoppingDistance)
			{
				controller.Move(agent.velocity * Time.deltaTime);
				animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
				return;
			}
		}

		stateMachine.ChangeState<IdleState>();
	}

	public override void OnExit()
	{
		animator?.SetBool(hashSlimeMove, false);
		animator?.SetBool(hashSlimeIdle, true);
		animator?.SetFloat(hashMoveSpeed, 0f);
		agent.ResetPath();
	}
}
