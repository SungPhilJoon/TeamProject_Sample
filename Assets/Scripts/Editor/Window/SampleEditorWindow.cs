using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SampleEditorWindow : EditorWindow
{
	public Rect windowRect = new Rect(100, 100, 200, 200);
	private float myFloat = 1.23f;
	private GameObject player;
	private ChatacterMoveController controller;

	void OnGUI()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		controller = player?.GetComponent<ChatacterMoveController>();

		BeginWindows();

		windowRect = GUILayout.Window(1, windowRect, DoWindow, "Hi There");
		controller.speed = EditorGUILayout.Slider("Slider", controller.speed, -10, 10);

		EndWindows();
	}

	void DoWindow(int unusedWindowID)
	{
		GUILayout.Button("Hi");
		GUI.DragWindow();
	}

	[MenuItem("Example/BeginWindow")]
	static void Init()
	{
		GetWindow(typeof(SampleEditorWindow));
	}
}
