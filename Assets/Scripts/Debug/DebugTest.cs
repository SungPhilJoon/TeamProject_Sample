using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
	public Animator animator;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle00")); ;
		}
    }
}
