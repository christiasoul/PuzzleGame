using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class saveChangeBut : standardButton {

	[SerializeField]
	private saveControl myControl;

	override public void OnPointerUp(PointerEventData myMouse){
		if (held == true) {
			// change directory of save


		}

	}

}
