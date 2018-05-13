using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anchor : MonoBehaviour {

	[SerializeField]
	private bool isAnchored = false;
	private bool isConnected = true;
	private int posNum = -1; // Stores which number position it is attached to (0 for start) 

	[SerializeField]
	private myRope heldRope;

	private float tickAngleMultiple;
	private float curTickAngle;

	// Use this for initialization
	void Start () {
		anchorTo ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void anchorTo(){
		if (heldRope) {
			posNum = heldRope.attach ();
		}

		setAnchor ();
	}

	public bool checkAnchored(){
		return isAnchored;
	}

	private void setAnchor(){
		if (heldRope) {
			heldRope.setAnchor (posNum, isAnchored);
		}

	}

	private void rotate(bool clockWise, int tickNum){


	}

	private void disconnectConnected(){


	}

	void OnMouseOver(){

	}

	void OnMouseDown(){


	}
}
