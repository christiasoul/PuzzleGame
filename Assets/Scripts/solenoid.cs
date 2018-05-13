using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A form of magnet that is created from electrical current through a coiled wire
public class solenoid : MonoBehaviour {

	private float turnsPerLength = 50;
	private float currentFlow;
	private float relativePermeabilityOfCore; // Depends on the material
	private float planetMagnetism;
	private float energyCost; // Claculated
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// 
	public float GetField(){
		return (turnsPerLength * currentFlow * relativePermeabilityOfCore) - planetMagnetism;
	}

	public float GetEnergyCost(){
		return energyCost;
	}
}
