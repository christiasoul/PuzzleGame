using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleMenu : MonoBehaviour {

	public GameObject playCanvas;

	// Use this for initialization
	void Start () {
		Debug.LogWarning ("Only clear curSave when loading or changing gametypes");
		controlSingle.Instance.LoadOptions ();
		saveControl.NewCurrentSave ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartNewGame(){

		// Create curSaveFile
		
	}

	public void FreePlay(){

		controlSingle.Instance.FreePlay (playCanvas);

	}
}
