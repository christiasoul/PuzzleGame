using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class placementObject {

	private int id;
	private int index;
	private bool isPreset;

	// Could make prefabs into strings
	private GameObject myGamePrefab;
	private GameObject curObject;
	private placedObjectAnchor[] myAnchors;
	private Vector3 myPos;


	/*
	public placementObject(int setId, Vector3 setPos){
		myPos = setPos;
		id = setId;
		myControlPrefab = controlSingle.Instance.GetControlPrefab (setId);
	}*/

	public placementObject(int setIndex, Vector3 setPos, int myId){
		index = setIndex;
		myPos = setPos;
		myGamePrefab = placementControl.instance.GetObjectControl(myId).gameObject;

		// Get Info from prefabs
		//placedObjectControl myInfo = myControlPrefab.GetComponent<placedObjectControl>();

		id = myId;


	}

	public GameObject GetControlObject(){

		return myGamePrefab;
	}

	public GameObject GetPhysicsObject(){
		return myGamePrefab.GetComponent<placedObjectControl> ().GetPhysicsObject ();
	}

	public void SetIndex(int newIndex){
		index = newIndex;
	}

	public int GetIndex(){
		return index;
	}

	public bool CheckIsPreset(){
		return isPreset;
	}

	public void CreateControlOb(){
		if (curObject != null) {
			GameObject.Destroy (curObject);
		}
		curObject = GameObject.Instantiate (myGamePrefab);
		curObject.transform.position = myPos;
		curObject.GetComponent<placedObjectControl> ().SetMyIndex (index);
	}

	public void CreatePhysicsOb(){
		if (curObject != null) {
			GameObject.Destroy (curObject);
		}
		curObject = GameObject.Instantiate (myGamePrefab.GetComponent<placedObjectControl>().GetPhysicsObject());
		curObject.transform.position = myPos;
	}

	public void Clear(){
		GameObject.Destroy (curObject);
	}

	public void SetCurObject(GameObject newOb){
		if (newOb != null) {
			Debug.LogError ("Oh hey theres a placement object thats losing a reference to its current object\nDont do that");
		} else {
			curObject = newOb;
		}
	}

	public int GetNumOfConnections(){
		return myGamePrefab.GetComponent<placedObjectControl> ().GetNumOfConnections ();
	}

	public bool GetIsOverlappable(){
		return myGamePrefab.GetComponent<placedObjectControl> ().GetIsOverlappable ();
	}

	public int GetId(){
		return id;
	}

	public Vector3 GetPos(){
		return myPos;
	}

	public GameObject GetGameObject(){
		return myGamePrefab;
	}

	public placementObject [] GetConnections(){
		return myAnchors.Get;
	}

	//Connect the new object in position connectPos
	public bool Connect(placementObject newOb, int connectPos){
		if (connectPos >= GetNumOfConnections()) {
			return false;
		} else {
			connections [connectPos] = newOb;
			return true;
		}
	}

	public void ReadyForSave(){

	}

	public virtual int GetExtensionType(){
		return -1;
	}
}
