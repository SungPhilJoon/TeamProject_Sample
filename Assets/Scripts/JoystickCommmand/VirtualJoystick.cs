using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	#region Variables
	[SerializeField]
	private RectTransform lever;
	private RectTransform rectTransform;

	[SerializeField, Range(10f, 150f)]
	private float leverRange;

	private Vector2 inputVector;
	private bool isInput;

	public enum JoystickType { Move, Rotate }
	public JoystickType joystickType;

	[SerializeField]
	private RPGCharacterController controller;

	#endregion Variables

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		ControlJoystickLever(eventData);

		isInput = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		ControlJoystickLever(eventData);
	}

	public void ControlJoystickLever(PointerEventData eventData)
	{
		Vector2 inputDir = eventData.position - rectTransform.anchoredPosition;
		Vector2 clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
		lever.anchoredPosition = clampedDir;
		inputVector = clampedDir / leverRange;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		lever.anchoredPosition = Vector2.zero;
		isInput = false;

		controller.Move(Vector2.zero);
	}

	private void InputControlVector()
	{
		switch (joystickType)
		{
			case JoystickType.Move:
				controller?.Move(inputVector);
				break;
			case JoystickType.Rotate:
				controller?.LookAround(inputVector);
				break;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (isInput)
		{
			InputControlVector();
		}
    }
}
