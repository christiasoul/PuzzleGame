using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class saveListButton : MonoBehaviour {
	
	private saveControl myControl;
	private int index;
	[SerializeField]
	private UnityEngine.UI.Text myText;

	public void SetMe(saveControl myNewBox, int myNewIndex, string setText){
		myControl = myNewBox;
		index = myNewIndex;
		myText.text = setText;
	}

	public void SaveClick(){
		myControl.SaveItemClick(index);
	}

}

/*
public class saveListButton : standardButton {

	private saveControl myControl;
	private int index;

	override public void OnPointerDown(PointerEventData myMouse){
		if (held == true) {
			// change directory of save
			myControl.SaveItemClick(index);

		}

	}

	public void SetMe(saveControl myNewBox, int myNewIndex){
		myControl = myNewBox;
		index = myNewIndex;
	}

}
*/