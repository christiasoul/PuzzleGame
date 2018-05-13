using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control the physics of the stage
public class physicsGameControl : MonoBehaviour {

	private const float winDelay = .15f;
	private float winTime = 0;
	private bool hasWon = false;
	[SerializeField]
	private GameObject gameFinishScreen;

	private GameObject myFinishScreen;

	// Stores number of Objects of each type
	private int[] numOfObjects;
	[SerializeField]
	private physicsWinCondition myCondition;
	public static physicsGameControl instance;

	private float levelFrictionCoef;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () { 
		if (hasWon) {
			if (winTime >= winDelay) {
				DisplayWinScreen ();
				Disable ();
			} else {
				winTime += Time.deltaTime;
			}
		} else {
			if (CheckWinCondition ()) {
				hasWon = true;

			}
		}
	}

	private bool CheckWinCondition(){
		return myCondition.CheckWinCondition ();

	}

	public float GetFrictionCoef(){
		return levelFrictionCoef;
	}

	public int GetSingleObjectNum(int arrayNum){
		return numOfObjects [arrayNum];
	}

	public int [] GetAllObjectNum(){
		return numOfObjects;
	}

	public void DisplayWinScreen(){
		myFinishScreen = GameObject.Instantiate (gameFinishScreen);
	}

	public void Disable(){
		enabled = false;
	}

	public void Reenable(){
		enabled = true;
	}

	public void Cleanup(){

	}
}
