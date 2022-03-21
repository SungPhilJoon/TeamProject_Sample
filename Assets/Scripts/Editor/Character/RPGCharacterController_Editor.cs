using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RPGCharacterController))]
public class RPGCharacterController_Editor : Editor
{
	#region Variables
	private RPGCharacterController targetController;

	#endregion Variables


	public override void OnInspectorGUI()
	{
		targetController = (RPGCharacterController)target;
		base.OnInspectorGUI();
	}

	void OnSceneGUI()
	{
		if (!targetController || !targetController.Camera)
		{
			return;
		}

		Transform targetCamera = targetController.Camera;

		Handles.color = new Color(0f, 0f, 1f, 0.5f);
		Vector3 cameraPosition = targetController.transform.position;
		cameraPosition.y += 2f;
		float distance = -targetCamera.localPosition.z;
		distance = Handles.ScaleSlider(distance, 
			cameraPosition,
			-targetController.transform.forward,
			Quaternion.identity,
			distance, 0.1f);
		distance = Mathf.Clamp(distance, 2.0f, 10.0f);
		targetCamera.localPosition = new Vector3(targetCamera.localPosition.x, targetCamera.localPosition.y, -distance);

		Handles.color = new Color(0f, 1f, 0f, 0.5f);
		float height = targetCamera.localPosition.y;
		height = Handles.ScaleSlider(height,
			cameraPosition,
			Vector3.up,
			Quaternion.identity,
			height, 0.1f);
		height = Mathf.Clamp(height, 1.0f, 5.0f);
		targetCamera.localPosition = new Vector3(targetCamera.localPosition.x, height, targetCamera.localPosition.z);

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.fontSize = 15;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.UpperCenter;

		Handles.Label(cameraPosition + (-targetController.transform.forward * distance), "Distance", labelStyle);

		labelStyle.alignment = TextAnchor.MiddleLeft;
		Handles.Label(cameraPosition + (Vector3.up * height), "Height", labelStyle);
	}
}
