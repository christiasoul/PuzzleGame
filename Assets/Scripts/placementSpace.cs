using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class placementSpace {

	private List<placementObject> myObs = new List<placementObject>();
	private List<placementObject> presetObs = new List<placementObject>();
	private Vector3 screenSpace; // The size of the Screen
	private Vector3 screenOffset;
	private static float cameraSize;
	private int spaceID; // used for loading and saving

	private bool isInPhysics = false;

	private int moneyLimit = 1000;
	private int itemLimit = 1000;
	private int moneyCurrent = 0;


	public placementSpace(List<placementObject> playerObs, List<placementObject> setObs, Vector3 screenSize, Vector3 newLastScreenOffset, int myId){
		
		cameraSize = Camera.main.orthographicSize;

		if (playerObs != null) {
			myObs = playerObs;

		}
		if (setObs != null) {
			presetObs = setObs;
		}

		if (newLastScreenOffset == null || newLastScreenOffset == Vector3.zero) {
			screenOffset = new Vector3 ((screenSize.x / 2), (screenSize.y / 2), -10f);

		} else {
			screenOffset = newLastScreenOffset;
		}

		screenSpace = screenSize;
		spaceID = myId;
		placementControl.instance.SetCamera (screenOffset);
		
	}

	public void AddItem(Vector3 itemPos, int setId){
		myObs.Add(new placementObject(myObs.Count, itemPos, setId));
		myObs [myObs.Count - 1].CreateControlOb (); 
		moneyCurrent += placementControl.instance.GetObjectControl (myObs [myObs.Count - 1].GetId ()).GetMyCost ();
		UpdateMoney ();
	}

	public void FlipPhysics(){
		if (isInPhysics) {
			TurnSetupMode ();
		} else {
			TurnPhysicsMode ();
		}
	}

	public void TurnPhysicsMode(){
		for (int i = 0; i < myObs.Count; i++) {
			myObs [i].CreatePhysicsOb ();
		}
		for (int i = 0; i < presetObs.Count; i++) {
			presetObs [i].CreatePhysicsOb ();
		}
		placementControl.instance.ClearObject ();
		isInPhysics = true;
	}

	public void TurnSetupMode(){
		for (int i = 0; i < myObs.Count; i++) {
			myObs [i].CreateControlOb();
		}
		for (int i = 0; i < presetObs.Count; i++) {
			presetObs [i].CreateControlOb();
		}

		isInPhysics = false;
	}

	public bool IsInPhysicsMode(){
		return isInPhysics;
	}

	public List<placementObject> GetObjects(){
		return myObs;
	}

	public List<placementObject> GetPresetObjects (){
		return presetObs;
	}

	public void RemoveItem(int index){
		if (controlSingle.Instance.IsFreePlay ()) {
			moneyCurrent -= placementControl.instance.GetObjectControl (myObs [index].GetId ()).GetMyCost ();
			UpdateMoney ();
		} else {

		}

		if (placementControl.instance.GetSelectedObjectControl ()) {
			if (placementControl.instance.GetSelectedObjectControl ().GetMyIndex() == index) {
				placementControl.instance.ClearSelection ();
			}
		}

		myObs [index].Clear ();
		myObs.RemoveAt (index);
	}

	public Vector3 GetScreenSize(){
		return screenSpace;
	}

	public void SetScreenSize(Vector3 newSize){
		screenSpace = newSize;
	}

	// Only use in editor to make new levels
	public void TurnIntoPreset(){
		for (int i = 0; i < myObs.Count; i++) {
			presetObs.Add (myObs [i]);
		}

		myObs.Clear ();
	}

	public void SetMoneyLimit(int newMoneyLimit){
		moneyLimit = newMoneyLimit;
	}

	public int GetPresetLength(){
		return presetObs.Count;
	}

	public int GetMyObsLength(){
		return myObs.Count;
	}

	public Vector3 GetScreenOffset(){
		return screenOffset;
	}

	public void SetScreenOffset(Vector3 newScreenOffset){
		screenOffset = newScreenOffset;
	}

	public void UpdateMoney(int newMoney){
		moneyCurrent = newMoney;
		UpdateMoney ();
	}

	public void UpdateMoney(){
		if (controlSingle.Instance.IsFreePlay ()) {
			placementControl.instance.UpdateMoneyDisplay (moneyCurrent);
		} else {
			placementControl.instance.UpdateMoneyDisplay (moneyLimit - moneyCurrent);
		}
	}

	public int getID(){
		return spaceID;
	}

	public bool CheckAbleToPlaceObject(placedObjectControl checkOb){
		if (!controlSingle.Instance.IsFreePlay() && checkOb.GetMyCost () + moneyCurrent > moneyLimit) {
			placementControl.instance.CreateNoMoneyAlert ();
			return false;
		} else if (myObs.Count >= itemLimit) {
			placementControl.instance.CreateNoItemAlert ();
			return false;
		}else {
			return true;
		}

	}

	public static void UpdateCameraSize(){
		cameraSize = Camera.current.orthographicSize;
	}

	public bool CanIncreaseScreenSize(float inAmt){
		float totalVec =  (cameraSize * inAmt);
		if (screenOffset.x - totalVec < 0 || screenOffset.y - totalVec < 0 ||
		    screenOffset.x + totalVec > screenSpace.x || screenOffset.y + totalVec > screenSpace.y) {
			return false;
		} else {
			return true;
		}
	}

	public bool CanMoveDir(Vector3 movement){
		Vector3 totalVec = movement + screenOffset;
			if (totalVec.x - (cameraSize) < 0 || totalVec.y - (cameraSize) < 0 ||
			totalVec.x + cameraSize > screenSpace.x || totalVec.y + cameraSize > screenSpace.y) {
			return false;
		} else {
			return true;
		}

	}

}