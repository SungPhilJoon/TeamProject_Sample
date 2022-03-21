using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
	#region Variables
	protected Transform target;

	#endregion Variables
	void Start()
    {
		HandleCamera();
	}

    void Update()
    {
		HandleCamera();
	}

	protected virtual void HandleCamera()
	{
	}
}
