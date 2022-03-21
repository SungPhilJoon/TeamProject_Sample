using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownCamera))]
public class TopDownCamera_SceneEditor : Editor
{
	#region Variables
	private TopDownCamera targetCamera;

	#endregion Variables

	public override void OnInspectorGUI()
	{
		targetCamera = (TopDownCamera)target;
		base.OnInspectorGUI();
	}

	void OnSceneGUI()
	{
		if (!targetCamera || !targetCamera.Target)
		{
			return;
		}

		Transform cameraTarget = targetCamera.Target;
		Vector3 targetPosition = cameraTarget.position;
		targetPosition.y += targetCamera.lookAtHeight;

		// Draw distance circle
		Handles.color = new Color(1f, 0f, 0f, 0.15f);
		Handles.DrawSolidDisc(targetPosition, Vector3.up, targetCamera.distance);

		Handles.color = new Color(0f, 1f, 0f, 0.75f);
		Handles.DrawWireDisc(targetPosition, Vector3.up, targetCamera.distance);

		// Create slider handles to adjust camera properties
		Handles.color = new Color(1f, 0f, 0f, 0.5f);
		targetCamera.distance = Handles.ScaleSlider(targetCamera.distance, 
			targetPosition, 
			-cameraTarget.forward, 
			Quaternion.identity, 
			targetCamera.distance, 0.1f);
		targetCamera.distance = Mathf.Clamp(targetCamera.distance, 2.0f, 20.0f);

		Handles.color = new Color(0f, 0f, 1f, 0.5f);
		targetCamera.height = Handles.ScaleSlider(targetCamera.height, 
			targetPosition, 
			Vector3.up, 
			Quaternion.identity, 
			targetCamera.height, 0.1f);
		targetCamera.height = Mathf.Clamp(targetCamera.height, 2.0f, 15.0f);

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.fontSize = 15;
		labelStyle.normal.textColor = Color.white;
		labelStyle.alignment = TextAnchor.UpperCenter;

		Handles.Label(targetPosition + (-cameraTarget.forward * targetCamera.distance), "Distance", labelStyle);

		labelStyle.alignment = TextAnchor.MiddleLeft;
		Handles.Label(targetPosition + (Vector3.up * targetCamera.height), "Height", labelStyle);

		targetCamera.HandleCamera();
	}
}
