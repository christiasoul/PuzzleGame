using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class connectorSaveInfo : objectSaveInfo {

	private List<int> myConnections;

	public connectorSaveInfo(List<int> newConnections){
		myConnections = newConnections;
	}

	public connectorSaveInfo(){
		myConnections = new List<int> ();
	}

	public override int GetSaveType ()
	{
		return 0;
	}

	public List<int> GetAllConnections(){
		return myConnections;
	}

	public int GetConnection(int index){
		return myConnections [index];
	}

	public void SetConnections(List<int> newConnections){
		myConnections = newConnections;
	}

	public void SetSingleConnection(int newConnection){
		myConnections.Add (newConnection);
	}

	public void RemoveConnection(int removedConnection){
		myConnections.Remove (removedConnection);
	}

}
