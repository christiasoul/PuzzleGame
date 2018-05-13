using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used in the setup phase and control a few things
// store all things that arent saved between saves here
public class placedObjectControl : MonoBehaviour {
	
	[SerializeField]
	private int myId;
	[SerializeField]
	private string myName;
	[SerializeField]
	private bool isOverlappable; // may replace with array of bools for 
	[SerializeField]
	private int numOfConnections;
	[SerializeField]
	private int myCost;
	[SerializeField]
	private GameObject myPhysicsObject;

	private static GameObject infoPrefab;

	private int myIndex;
	private Color myColor;
	private GameObject myInfoBlock;
	// Use this for initialization
	void Awake () {
		myColor = GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMyIndex(int newIndex){
		myIndex = newIndex;
	}

	public static void SetMyInfoPrefab(GameObject newInfo){
		infoPrefab = newInfo;
	}

	public bool CheckIsOverlappable(){
		return isOverlappable;
	}
		
	public int GetId(){
		return myId;
	}

	public int GetNumOfConnections(){
		return numOfConnections;
	}

	public bool GetIsOverlappable(){
		return isOverlappable;
	}

	public int GetMyCost(){
		return myCost;
	}

	public int GetMyIndex(){
		return myIndex;
	}

	public GameObject GetPhysicsObject(){
		return myPhysicsObject;
	}

	public Color GetMyColor(){
		return myColor;
	}

	public void DeleteObject(){
		placementControl.instance.GetMySpace ().RemoveItem (myIndex);
	}

	public void ClearInfoBlock(){
		if (myInfoBlock != null) {
			Destroy (myInfoBlock);
		}
	}

	public void CreateInfoBlock(){
		myInfoBlock = Instantiate (infoPrefab, controlSingle.Instance.GetCanvas().transform);
		string myInfoString = 
			myName +
			"\nCost : " + myCost
			;
		myInfoBlock.GetComponentInChildren<UnityEngine.UI.Text> ().text = myInfoString;
		
	}

}

public enum objectType{
	ITEM,
	FIELD
}