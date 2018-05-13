using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placedObjectAnchor : MonoBehaviour {

	[SerializeField]
	private int myIndex;
	[SerializeField]
	private GameObject mainObject;
	[SerializeField]
	private string toolBoxInfo;
	 
	private int myControlIndex = -1;

	private List<int> objectConnectIndexes;

	public int GetControlIndex(){
		return myControlIndex;
	}

	public void SetControlIndex(int newIndex){
		myControlIndex = newIndex;
	}

	public int [] GetConnectedIndexes(){
		return objectConnectIndexes.ToArray ();
	}

	// Do immediately
	public void RemoveConnectedIndex(int removedIndex){
		objectConnectIndexes.Remove (removedIndex);
	}

	public connectorSaveInfo mySaveData;

}
