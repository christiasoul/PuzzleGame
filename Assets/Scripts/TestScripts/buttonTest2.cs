using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTest2 : MonoBehaviour {

	public void ResumeButton(){
		controlSingle.Instance.RemoveMenu ();
		controlSingle.Instance.ResumeGame ();
	}
}
