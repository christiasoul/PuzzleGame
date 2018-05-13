using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masterInput : MonoBehaviour {

	private GameObject currentMenu;
	public static masterInput instance = null;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MainMenuEsc ();	
	}

	public void MainMenuEsc(){
		if (Input.GetButtonDown("Main Menu")){

			// !!! IF something is current menu, get rid of it, also go down a level/exit 

			if (controlSingle.Instance.IsMenu() == false){
				controlSingle.Instance.CreateMenuPopup ();
			}else{
				controlSingle.Instance.RemoveMenu ();
			}
		}
	}
}
