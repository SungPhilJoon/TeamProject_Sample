using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
	#region Variables
	[Header("Movement Variables")]
	public float speed = 5.0f; // �̵� �ӵ�
	public float jumpHeight = 2.0f; // ���� ����
	public float dashDistance = 5.0f; // �뽬 �Ÿ�

	private Rigidbody myRigidbody; // ĳ���� Rigidbody������ ����

	private Vector3 inputDirection = Vector3.zero; // �̵� �Է¿� ���� ������ ��Ÿ���� ����

	private bool isGrounded = false; // ĳ���Ͱ� ���� �ִ��� Ȯ���ϴ� ����
	public LayerMask groundLayerMask; // ���� �ִ��� Ȯ���ϱ� ���� �������� �־��ֱ� ���� LayerMask�� ����ߴ�.
	public float groundCheckDistance = 0.3f; // ���� �ִ��� �����ϱ� ���� �Ÿ� ����

	#endregion Variables

	void Start()
    {
		myRigidbody = GetComponent<Rigidbody>(); // Rigidbody�� ĳ��
    }

    // Update is called once per frame
    void Update()
    {
		if (myRigidbody == null) return;

		CheckGroundStatus();

		// ĳ���� �̵� �Է�
		inputDirection = Vector3.zero;
		inputDirection.x = Input.GetAxis("Horizontal");
		inputDirection.z = Input.GetAxis("Vertical");
		if (inputDirection != Vector3.zero)
		{
			transform.forward = inputDirection;
		}

		// ĳ���� ���� �Է�
		if (Input.GetButtonDown("Jump") && isGrounded) // ���� ���� ��쿡�� ���� ����
		{
			Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
			myRigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
		}

		// ĳ���� �뽬 �Է�
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
