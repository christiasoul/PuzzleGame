using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class menuScreen : MonoBehaviour {

	protected menuScreen prevMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract void Cleanup ();

	// Cleanup every menu to resume game
	public void CleanToBase(){
		if (prevMenu != null) {
			prevMenu.CleanToBase ();
		} else {
			controlSingle.Instance.ResumeGame ();
			controlSingle.Instance.SetMenu (null);
		}

		Destroy (gameObject);
			
	}

	public void SetPrevMenu(menuScreen newPrev){
		prevMenu = newPrev;
	}

	protected GameObject CreateNextMenu(GameObject creationObject){
		GameObject myOb = GameObject.Instantiate (creationObject, controlSingle.Instance.GetCanvas ().transform);
		JumpForward (myOb.GetComponent<menuScreen> ());
		return myOb;
	}

	protected void JumpBackMenu(){
		prevMenu.JumpMe ();
		controlSingle.Instance.SetMenu (prevMenu);
	}

	protected void JumpForward(menuScreen newTarg){
		Disable ();
		controlSingle.Instance.SetMenu (newTarg);
		newTarg.SetPrevMenu (this);
		newTarg.JumpMe();
	}

	public void JumpMe(){
		Enable ();
	}

	private void Disable(){
		enabled = false;
	}

	private void Enable(){
		enabled = true;
	}

}
