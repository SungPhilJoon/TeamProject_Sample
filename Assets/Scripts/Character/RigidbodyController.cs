using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
	#region Variables
	[Header("Movement Variables")]
	public float speed = 5.0f; // 이동 속도
	public float jumpHeight = 2.0f; // 점프 높이
	public float dashDistance = 5.0f; // 대쉬 거리

	private Rigidbody myRigidbody; // 캐싱할 Rigidbody변수를 생성

	private Vector3 inputDirection = Vector3.zero; // 이동 입력에 대한 방향을 나타내는 변수

	private bool isGrounded = false; // 캐릭터가 땅에 있는지 확인하는 변수
	public LayerMask groundLayerMask; // 땅에 있는지 확인하기 위한 디테일을 넣어주기 위해 LayerMask를 사용했다.
	public float groundCheckDistance = 0.3f; // 땅에 있는지 측정하기 위한 거리 변수

	#endregion Variables

	void Start()
    {
		myRigidbody = GetComponent<Rigidbody>(); // Rigidbody를 캐싱
    }

    // Update is called once per frame
    void Update()
    {
		if (myRigidbody == null) return;

		CheckGroundStatus();

		// 캐릭터 이동 입력
		inputDirection = Vector3.zero;
		inputDirection.x = Input.GetAxis("Horizontal");
		inputDirection.z = Input.GetAxis("Vertical");
		if (inputDirection != Vector3.zero)
		{
			transform.forward = inputDirection;
		}

		// 캐릭터 점프 입력
		if (Input.GetButtonDown("Jump") && isGrounded) // 땅에 있을 경우에만 점프 가능
		{
			Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
			myRigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
		}

		// 캐릭터 대쉬 입력
		if (Input.GetButtonDown("Dash"))
		{
			Vector3 dashVelocity = Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (myRigidbody.drag * Time.deltaTime + 1)) / -Time.deltaTime),
				0,
				(Mathf.Log(1 / (myRigidbody.drag * Time.deltaTime + 1)) / -Time.deltaTime)));
			myRigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
		}
	}

	void FixedUpdate()
	{
		myRigidbody.MovePosition(myRigidbody.position + inputDirection * speed * Time.fixedDeltaTime);
	}

	#region Helper Methods
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;

#if UNITY_EDITOR
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f),
			transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance), Color.red);
#endif

		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance, groundLayerMask))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}
	#endregion Helper Methods
}
