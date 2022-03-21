using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RPGCharacterController : MonoBehaviour
{
	#region Variables
	[SerializeField]
	private Transform characterBody; // 캐릭터
	[SerializeField]
	private Transform cameraArm;    // 캐릭터 메인 카메라 암
	[SerializeField]
	private Transform camera;

	public Animator animator;

	// 캐릭터 기본 속도
	public float baseSpeed = 5f;

	// 애니메티어 파라미터 캐싱
	public readonly int hashIdleIndex = Animator.StringToHash("IdleIndex");
	public readonly int hashMove = Animator.StringToHash("Move");

	#endregion Variables

	#region Properties
	public Transform Camera => camera;

    #endregion Properties

    void Awake()
	{
		animator = GetComponentInChildren<Animator>();
	}

	public void Move(Vector2 inputVector)
	{
		bool isMove = inputVector.sqrMagnitude != 0;
		animator.SetBool(hashMove, isMove);

		if (isMove)
		{
			Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
			Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
			Vector3 moveDir = lookForward * inputVector.y + lookRight * inputVector.x;

			characterBody.forward = moveDir;
			transform.position += moveDir * baseSpeed * Time.deltaTime;
		}
	}

	public void LookAround(Vector2 inputVector)
	{
		Vector3 cameraAngle = cameraArm.rotation.eulerAngles;

		float x = cameraAngle.x - inputVector.y;

		if (x < 180)
		{
			x = Mathf.Clamp(x, -1f, 70f);
		}
		else
		{
			x = Mathf.Clamp(x, 355f, 361f);
		}

		cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + inputVector.x, cameraAngle.z);
	}
}
