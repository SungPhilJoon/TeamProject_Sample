using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatacterMoveController : MonoBehaviour
{
	#region Variables
	[Header("Movement Variables")]
	public float speed = 5.0f; // �̵� �ӵ�
	public float jumpHeight = 2.0f; // ���� ����
	public float dashDistance = 5.0f; // �뽬 �Ÿ�

	private CharacterController characterController; // ĳ���� CharacterController���� ����

	public float gravity = -29.81f;
	public Vector3 drags;

	private bool isGrounded = false; // ĳ���Ͱ� ���� �ִ��� Ȯ���ϴ� ����
	public LayerMask groundLayerMask; // ���� �ִ��� Ȯ���ϱ� ���� �������� �־��ֱ� ���� LayerMask�� ����ߴ�.
	public float groundCheckDistance = 0.3f; // ���� �ִ��� �����ϱ� ���� �Ÿ� ����

	private Vector3 calcVelocity;
	
	#endregion Variables

	// Start is called before the first frame update
	void Start()
    {
		characterController = GetComponent<CharacterController>();
	}

    // Update is called once per frame
    void Update()
    {
		isGrounded = characterController.isGrounded;
		if (isGrounded && calcVelocity.y < 0)
		{
			calcVelocity.y = 0;
		}

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		characterController.Move(move * Time.deltaTime * speed);
		if (move != Vector3.zero)
		{
			transform.forward = move;
		}

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
		}

		if (Input.GetButtonDown("Dash"))
		{
			Vector3 dashVelocity = Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1 / (drags.x * Time.deltaTime + 1)) / -Time.deltaTime),
				0,
				(Mathf.Log(1 / (drags.z * Time.deltaTime + 1)) / -Time.deltaTime)));
			calcVelocity += dashVelocity;
		}

		// Progress gravity
		calcVelocity.y += gravity * Time.deltaTime;

		// Progress dash ground drags
		calcVelocity.x /= 1 + drags.x * Time.deltaTime;
		calcVelocity.y /= 1 + drags.y * Time.deltaTime;
		calcVelocity.z /= 1 + drags.z * Time.deltaTime;

		characterController.Move(calcVelocity * Time.deltaTime);
	}
}
