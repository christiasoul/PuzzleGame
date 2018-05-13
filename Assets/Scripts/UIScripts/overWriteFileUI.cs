using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overWriteFileUI : menuScreen {
	
	private saveControl myControl;
	private bool myAnswer = false;

	public void SetControl(saveControl newControl){
		myControl = newControl;
	}

	public void AnswerYes(){
		myAnswer = true;
		Cleanup ();
	}

	public void AnswerNo(){
		Cleanup ();
	}


	public override void Cleanup(){
		myControl.OverWriteFile (myAnswer);
		JumpBackMenu ();
		Destroy (gameObject);
	}
		
}