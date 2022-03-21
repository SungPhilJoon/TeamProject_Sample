using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
	#region Variables
	public float height = 5f;
	public float distance = 10f;
	// public float angle = 45f;
	public float lookAtHeight = 2f;
	public float smoothSpeed = 0.5f;
	[SerializeField]
	private Transform target;

	private Vector3 refVelocity;
	#endregion Variables

	#region Properties
	public Transform Target => target;

	#endregion Properties

	void Start()
	{
		HandleCamera();
	}

	void LateUpdate()
	{
		HandleCamera();
	}

	[SerializeField]
	public void HandleCamera()
	{
		if (!target)
		{
			return;
		}

		Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);

		Vector3 flatTargetPosition = target.position;
		flatTargetPosition.y += lookAtHeight;

		Vector3 finalPosition = flatTargetPosition + worldPosition;

		// transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, finalPosition, smoothSpeed);
		transform.position = smoothedPosition;

		transform.LookAt(flatTargetPosition);
	}

	public void LookAround(Vector2 inputVector)
	{
		Vector3 rotatedX = Quaternion.AngleAxis(inputVector.x / 100f, target.up) * transform.position;
		Vector3 rotatedY = Quaternion.AngleAxis(inputVector.y / 100f, target.right) * transform.position;

		transform.position = (rotatedX + rotatedY);
	}
}
