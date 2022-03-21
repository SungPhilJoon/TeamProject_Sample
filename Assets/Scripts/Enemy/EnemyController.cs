using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IAttackable, IDamageable
{
	#region Variables
	protected StateMachine<EnemyController> stateMachine;
	public StateMachine<EnemyController> StateMachine => stateMachine;

	protected Animator animator;

	protected readonly int hashHitTrigger = Animator.StringToHash("HitTrigger");

	private FieldOfView fov;

	public LayerMask targetMask;
	// public Transform target;
	// public float viewRadius;
	public float attackRange;

	public Transform Target => fov?.NearestTarget;

	public Transform[] waypoints;
	[HideInInspector]
	public Transform targetWaypoint = null;
	private int waypointIndex = 0;

	public Transform projectileTransform;
	public Transform hitTransform;

    [SerializeField] private NPCBattleUI battleUI;

	public int maxHealth = 100;
	public int health;

	[SerializeField]
	private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

    public float AttackRange
    {
        get
        {
            return attackRange;
        }
        set
        {
            attackRange = CurrentAttackBehaviour?.range ?? 6.0f;
        }
    }

	#endregion Variables

	#region Unity Methods
	void Start()
    {
		stateMachine = new StateMachine<EnemyController>(this, new MoveToWayPoints());
		IdleState idleState = new IdleState();
		idleState.isPatrol = true;
		stateMachine.AddState(idleState);
		stateMachine.AddState(new MoveState());
		stateMachine.AddState(new AttackState());
		stateMachine.AddState(new DeadState());

		animator = GetComponent<Animator>();

		fov = GetComponent<FieldOfView>();

        health = maxHealth;

        if (battleUI != null)
        {
            battleUI.MinimumValue = 0.0f;
            battleUI.MaximumValue = maxHealth;
            battleUI.Value = health;
        }

        InitAttackBehaviour();
	}

    void Update()
    {
		CheckAttackBehaviour();
		stateMachine.Update(Time.deltaTime);
    }

	#endregion Unity Methods

	#region Helper Methods
    private void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            if (CurrentAttackBehaviour == null)
            {
                CurrentAttackBehaviour = behaviour;
            }

            behaviour.targetMask = targetMask;
        }
    }

    private void CheckAttackBehaviour()
    {
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
        {
            CurrentAttackBehaviour = null;

            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (behaviour.IsAvailable)
                {
                    if ((CurrentAttackBehaviour == null) ||
                        (CurrentAttackBehaviour.priority < behaviour.priority))
                    {
                        CurrentAttackBehaviour = behaviour;
                    }
                }
            }
        }
    }

	public bool IsAvailableAttack
	{
		get
		{
			if (!Target)
			{
				return false;
			}

			float distance = Vector3.Distance(transform.position, Target.position);
			return (distance <= attackRange);
		}
	}

	public Transform SearchEnemy()
	{
		//Target = null;

		//Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
		//if (targetInViewRadius.Length > 0)
		//{
		//	Target = targetInViewRadius[0].transform;
		//}

		return Target;
	}

	//void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawWireSphere(transform.position, viewRadius);

	//	Gizmos.color = Color.green;
	//	Gizmos.DrawWireSphere(transform.position, attackRange);
	//}

	public Transform FindNextWayPoint()
	{
		targetWaypoint = null;
		if (waypoints.Length > 0)
		{
			targetWaypoint = waypoints[waypointIndex];
		}

		waypointIndex = (waypointIndex + 1) % waypoints.Length;

		return targetWaypoint;
	}

	#endregion Helper Methods

	#region IAttackable interface
	public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null && Target != null)
        {
            CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectileTransform);
        }
    }

    #endregion IAttackable interface

    #region IDamageable interface
    public bool IsAlive => health > 0;

    public void TakeDamage(int damage, GameObject hitEffectPrefab)
    {
        if (!IsAlive)
        {
            return;
        }

        health -= damage;

        if (battleUI != null)
        {
            battleUI.Value = health;
        }

        if (hitEffectPrefab)
        {
            Instantiate(hitEffectPrefab, hitTransform);
        }

        if (IsAlive)
        {
            animator?.SetTrigger(hashHitTrigger);
        }
        else
        {
            if (battleUI != null)
            {
                battleUI.enabled = false;
            }

            stateMachine.ChangeState<DeadState>();
        }
    }

	#endregion IDamageable interface
}
