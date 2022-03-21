using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
	#region Variables
	private Animator animator;
    private AttackStateController attackStateController;
    private IAttackable attackable;

	protected readonly int hashAttack = Animator.StringToHash("Attack");
	protected readonly int hashAttackIndex = Animator.StringToHash("AttackIndex");

	#endregion Variables

	public override void OnInitialized()
	{
		animator = context.GetComponent<Animator>();
        attackStateController = context.GetComponent<AttackStateController>();
        attackable = context.GetComponent<IAttackable>();
    }

	public override void OnEnter()
	{
		if (attackable == null || attackable.CurrentAttackBehaviour == null)
        {
            stateMachine.ChangeState<IdleState>();
            return;
        }

        attackStateController.enterAttackStateHandler += OnEnterAttackState;
        attackStateController.exitAttackStateHandler += OnExitAttackState;

        animator?.SetInteger(hashAttackIndex, attackable.CurrentAttackBehaviour.animationIndex);
        animator?.SetTrigger(hashAttack);
    }

	public override void Update(float deltaTime)
	{
		
	}

    public override void OnExit()
    {
        attackStateController.enterAttackStateHandler -= OnEnterAttackState;
        attackStateController.exitAttackStateHandler -= OnExitAttackState;
    }

    public void OnEnterAttackState()
	{

	}

	public void OnExitAttackState()
	{
		stateMachine.ChangeState<IdleState>();
	}
}
