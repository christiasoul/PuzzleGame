using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class physicsWinCondition  {

	public abstract bool CheckWinCondition ();
	private GameObject [] winObjects;
	public void SetObjects (GameObject[] newObs){
		winObjects = newObs;

	}

}

/*
public class winConditionHeat : physicsWinCondition {
	public bool CheckWinCondition(){
		


	}

}
*/

// When a certain number of an item exists
public class winConditionItem : physicsWinCondition {
	[SerializeField]
	public physicsGameControl sceneControl;
	private Dictionary<int, int> itemRequirements;
	winConditionItem(int [] newItemRequirements, int [] newItemNum){
		Debug.Log ("physicsWinCondition Debug");
		if (newItemRequirements == null) {
			Debug.LogError ("The item win condition for this level is null");
		}

		itemRequirements = new Dictionary<int, int> ();

		for (int i = 0; i < newItemNum.Length; i++) {
			itemRequirements.Add (newItemNum [i], newItemRequirements [i]);

		}

	}
	public override bool CheckWinCondition(){
		foreach (KeyValuePair<int, int> item in itemRequirements){
			if (sceneControl.GetSingleObjectNum (item.Key) > item.Value) {
				return false;
			}
		}
		return true;


	}

}

public enum ItemList{
	STRUT,
	ROPE,
	LEVER,
	GEAR,
	MAGNET,
	ELECTROMAGNET

}

public enum OrbList{
	ENERGY,
	HEAT,
	MOVEMENT,
	FUSION,
	ENERGY_XL,
	HEAT_XL,
	MOVEMENT_XL

}