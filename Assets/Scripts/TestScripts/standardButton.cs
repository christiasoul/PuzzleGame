using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class standardButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

	[SerializeField]
	protected bool held = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	virtual public void OnPointerDown(PointerEventData myMouse){
		held = true;

		// Do visual

	}

	public void OnPointerExit(PointerEventData myMouse){
		if (held == true) {

			// Change back visul
			held = false;
		}

	}

	virtual public void OnPointerUp(PointerEventData myMouse){

	}
}
