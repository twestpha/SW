using UnityEngine;
using System.Collections;

public class FirstPersonMouseFixes : MonoBehaviour {

	private bool lockedAndHidden;

	void Start () {
		lockedAndHidden = true;
		setCursorState();
	}

	void FixedUpdate(){
		// TODO still a little buggy
		if(Input.GetKeyDown(KeyCode.Escape)){
			lockedAndHidden = !lockedAndHidden;
			setCursorState();
		}
	}

	void setCursorState(){
		Cursor.visible = !lockedAndHidden;
		Cursor.lockState = lockedAndHidden ? CursorLockMode.Locked : CursorLockMode.Confined;
	}
}
