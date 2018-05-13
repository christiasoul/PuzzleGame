using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class placementScreenMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

	private placementSpace mySpace;
	private bool isActive = false;

	[SerializeField]
	private MoveDirection moveDir;
	[SerializeField]
	private MoveDirection moveDir2;

	void Update(){
		if (isActive) {
			placementControl.instance.MoveAction(moveDir);
			if (moveDir2 != null && moveDir2 != MoveDirection.None) {
				placementControl.instance.MoveAction (moveDir2);
			}
		}
	}

	public void SetScreenSpace(placementSpace newSpace){
		mySpace = newSpace;
	}

	public void OnPointerEnter(PointerEventData eventData){
		isActive = true;
		Debug.Log ("Hey entered");

	}

	public void OnPointerExit(PointerEventData eventData){
		isActive = false;
	}
}
