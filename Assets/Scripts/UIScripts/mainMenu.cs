using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : menuScreen {
	[SerializeField]
	private GameObject saveScreenPrefab;
	[SerializeField]
	private GameObject loadScreenPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ResumeButton(){
		Cleanup ();
	}

	public void SaveButton(){
		CreateNextMenu (saveScreenPrefab);
	}

	public void LoadButton(){
		CreateNextMenu (loadScreenPrefab);
	}

	public void OptionsButton(){

	}

	public void HelpMenu(){

	}

	public void ExitToTitle(){

	}

	public void ExitGame(){

	}

	public override void Cleanup ()
	{
		controlSingle.Instance.ResumeGame ();
		controlSingle.Instance.SetMenu (null);
		Destroy (gameObject);
	}
}
