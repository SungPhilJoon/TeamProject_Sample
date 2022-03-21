using UnityEngine;

public class IdleState : State<EnemyController>
{
	#region Variables
	public bool isPatrol = true;
	private float minIdleTime = 7.0f;
	private float maxIdleTime = 8.0f;
	private float idleTime = 0.0f;

	private Animator animator;
	private CharacterController controller;

	protected readonly int hashSlimeMove = Animator.StringToHash("SlimeMove");
	protected readonly int hashMoveSpeed = Animator.StringToHash("MoveSpeed");
	protected readonly int hashSlimeIdle = Animator.StringToHash("SlimeIdle");

	#endregion Variables

	public override void OnInitialized()
	{
		animator = context.GetComponent<Animator>();
		controller = context.GetComponent<CharacterController>();
	}

	public override void OnEnter()
	{
		animator?.SetBool(hashSlimeIdle, true);
		animator?.SetBool(hashSlimeMove, false);
		animator?.SetFloat(hashMoveSpeed, 0f);
		controller?.Move(Vector3.zero);

		if (isPatrol)
		{
			idleTime = Random.Range(minIdleTime, maxIdleTime);
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
		else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)
		{
			stateMachine.ChangeState<MoveToWayPoints>();
		}
	}

	public override void OnExit()
	{
		animator?.SetBool(hashSlimeIdle, false);
	}
}
