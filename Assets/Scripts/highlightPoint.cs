using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlightPoint : MonoBehaviour {

	private GameObject myToolBox;
	private float myVal;
	[SerializeField]
	private static GameObject toolBoxPrefab;
	private static Vector3 cursorSize; // find cursor Size

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver(){
		if (myToolBox != null) {

		} else {
			myToolBox = GameObject.Instantiate (toolBoxPrefab);
		}

		dirOfToolBox ();
	}

	void OnMouseExit(){
		GameObject.Destroy (myToolBox);
	}

	private enum Dir{NORTH, EAST, SOUTH, WEST};

	private void dirOfToolBox(){
		Vector2 screenOffset = controlSingle.Instance.GetScreenOffset ();
		Vector2 screenSize = controlSingle.Instance.GetScreenSize ();
		RectTransform textBoxTrans = myToolBox.GetComponentInChildren<RectTransform> ();
		Vector3 textBoxSize = textBoxTrans.sizeDelta;
		Vector3 textBoxPos = Input.mousePosition;

		float xPos = textBoxPos.x + cursorSize.x + ( textBoxSize.x / 2 );
		float yPos = textBoxPos.y + cursorSize.y + ( textBoxSize.y / 2 );

		// Check Right
		if (textBoxPos.x + textBoxSize.x > screenSize.x + screenOffset.x ){
			xPos = -xPos;
		}
		else{
			xPos = xPos;
		}
		// check Top
		if (textBoxPos.y + textBoxSize.y > screenSize.y + screenOffset.y ){
			yPos = -yPos;
		}
		else{
			yPos = yPos;
		}

		myToolBox.GetComponent<RectTransform> ().localPosition = new Vector2(xPos, yPos);

	}
}
