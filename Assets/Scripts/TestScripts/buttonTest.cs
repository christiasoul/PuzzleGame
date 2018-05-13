using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class buttonTest : UnityEngine.Events.UnityEvent {

	static void ResumeButton(){
		controlSingle.Instance.RemoveMenu ();
		controlSingle.Instance.ResumeGame ();
	}

}
