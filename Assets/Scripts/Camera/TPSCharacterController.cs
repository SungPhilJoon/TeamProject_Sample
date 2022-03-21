using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharacterController : MonoBehaviour
{
	#region Variables
	[SerializeField]
	private Transform characterBody;
	[SerializeField]
	private Transform cameraArm;

	Animator animator;

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
		animator = characterBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	#region Helper Method
	public void LookAround(Vector2 inputVector)
	{
		Vector3 camAngle = cameraArm.rotation.eulerAngles;

		float x = camAngle.x - inputVector.y;
		if (x < 180)
		{
			x = Mathf.Clamp(x, -1f, 70f);
		}
		else
		{
			x = Mathf.Clamp(x, 335f, 361f);
		}

		cameraArm.rotation = Quaternion.Euler(x, camAngle.y + inputVector.x, camAngle.z);
	}

	#endregion Helper Method
}
