using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsObject : MonoBehaviour {

	private float frictionCoef;
	private float airDragPerMeter;
	private float mass;
	private float temperature;
	private float specificHeatCapacity;
	private float thermalConductivity;
	private float meltingFreezingPoint;
	private float evapDewPoint;
	private Vector3 velocity;
	private Vector3 heldForce;
	// Have something pointing to what it turns into
	// !!


	// Use this for initialization
	void Start () {
		frictionCoef = physicsGameControl.instance.GetFrictionCoef ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// After velocity needs to be updated, all held force will be added
	void ApplyForce(Vector3 directionalForce){
		heldForce += directionalForce;
	}



}
