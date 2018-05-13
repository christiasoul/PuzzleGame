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
		int checkId = checkOb.GetInstanceID ();

		connections.Contains (checkOb);

		for (int i = 0; i < connections.Count; i++){

		}
	}



}
