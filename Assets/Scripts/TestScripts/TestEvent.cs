using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TestEvent : MonoBehaviour
{
    public UnityEvent unityEvent;

    void Start()
    {
        unityEvent.Invoke();
    }
}
