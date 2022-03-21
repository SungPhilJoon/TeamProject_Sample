using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(NavMeshAgent)),RequireComponent(typeof(Animator))]
public class CharacterMoveAgent : MonoBehaviour
{
	#region Variables
	[SerializeField]
	// Stat

	private CharacterController controller;
	private NavMeshAgent agent;
	private Animator animator;
	private Camera camera;

	[SerializeField]
	private LayerMask groundLayerMask;

	private readonly int hashMove = Animator.StringToHash("Move");

	#endregion Variables

	#region Properties
	public CharacterController Controller => controller;

	#endregion Properties

	#region Unity Methods
	void Start()
    {
		controller = GetComponent<CharacterController>();

		agent = GetComponent<NavMeshAgent>();
		agent.updatePosition = false;
		agent.updateRotation = true;

		animator = GetComponent<Animator>();

		camera = Camera.main;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Jump"))
		{
			agent.speed = 100f;
			agent.acceleration = 100f;
		}

        if (Input.GetMouseButtonDown(1))
		{
			// Make ray from screen to world
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			agent.speed = 3.5f;
			agent.acceleration = 8f;

			// Check hit from ray
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
			{
				agent.SetDestination(hit.point);
			}
		}

		if (agent.remainingDistance > agent.stoppingDistance)
		{
			controller.Move(agent.velocity * Time.deltaTime);
			animator.SetBool(hashMove, true);
		}
		else
		{
			controller.Move(Vector3.zero);
			animator.SetBool(hashMove, false);
		}
    }

	void LateUpdate()
	{
		transform.position = agent.nextPosition;
	}

	#endregion Unity Methods
}
