using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linkedObjectControl {

	private int myId;

	private List<GameObject> connections;

	public linkedObjectControl(){
		myId = placementControl.instance.GetNewConnectId ();
		placementControl.instance.AddNewConnected (this);

	}

	public bool checkIfGameObject(GameObject checkOb){

		return connections.Contains (checkOb); // May be slow

	}



}
