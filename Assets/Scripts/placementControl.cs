using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class placementControl : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	[SerializeField]
	private placedObjectControl[] objectMasterList; 
	private GameObject curPrefabSelection;
	private placedObjectControl controlObjectSelection;
	private placementSpace curSpace;
	private bool isActive = false;
	private Vector3 curScreenOffset; // The distance from the bottom left of the screen from the bottom left of the stage
	[SerializeField]
	private Camera myCamera;
	[SerializeField]
	private GameObject alertMoneyOverPrefab;
	[SerializeField]
	private GameObject alertItemOverPrefab;
	[SerializeField]
	private GameObject alertNoSpacePrefab;
	[SerializeField]
	private GameObject moneyDisplayFree;
	[SerializeField]
	private GameObject moneyDisplayLimited;
	[SerializeField]
	private GameObject ObjectSelectionInfoBlock;
	private float itemClearanceArea = 1f;
	private GameObject myMoneyDisplay;

	[SerializeField]
	private GameObject [] buttonSectionList;

	[SerializeField]
	private float selectAccuracy = .1f;
	[SerializeField]
	private float cameraSmoothTime = 1f;

	public int buttonSectionSelection = -1;

	private List<MoveDirection> myMoveActions;

	public static placementControl instance = null;

	private bool isOverlap = false;
	private bool middleMouseDown = false;
	private Vector3 middleMousePosition;
	private Vector3 cameraVelocity;
	private float curCameraSmoothTime = 0f;

	private List<linkedObjectControl> allConnected;

	void Awake(){

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		myMoveActions = new List<MoveDirection> ();

		if (controlSingle.Instance.IsFreePlay ()) {
			myMoneyDisplay = Instantiate (moneyDisplayFree, controlSingle.Instance.GetCanvas ().transform);

		} else {
			myMoneyDisplay = Instantiate (moneyDisplayLimited, controlSingle.Instance.GetCanvas ().transform);
		}
		//!! TEMP


		placedObjectControl.SetMyInfoPrefab (ObjectSelectionInfoBlock);

	}

	public void StartFreePlay(Vector3 myScreenSpace){
		curSpace = new placementSpace(null, null, myScreenSpace, Vector3.zero, 0);
	}

	public void OnPointerUp(PointerEventData eventData){
		if (eventData.button == PointerEventData.InputButton.Right) {

			if (curPrefabSelection != null) {
				Debug.Log ("Right Button Press");
				ClearObject ();
			}
		} else if (eventData.button == PointerEventData.InputButton.Left) {
			if (curPrefabSelection != null) {
				Debug.Log ("Left Button Press");
				if (!CheckArea ()) {
					CreateNoSpaceAlert ();
				} else if (curSpace.CheckAbleToPlaceObject (curPrefabSelection.GetComponent<placedObjectControl> ())) {
					curSpace.AddItem (Vector3.ProjectOnPlane (myCamera.ScreenToWorldPoint (Input.mousePosition), Vector3.forward) + curScreenOffset, curPrefabSelection.GetComponent<placedObjectControl> ().GetId ());
					if (Input.GetButton ("MultipleObjectHold") != true) {
						ClearObject ();
					}
				} else {
				
				}
			}  else {
				Collider2D [] hitRet = Physics2D.OverlapCircleAll (Vector3.ProjectOnPlane (myCamera.ScreenToWorldPoint (Input.mousePosition), Vector3.forward), selectAccuracy);
				if (hitRet.Length > 0 )
					
				SetMyControlObjectSelection( GetPriorityObject( hitRet ).GetComponent<placedObjectControl>() );
			}
		} else if (eventData.button == PointerEventData.InputButton.Middle) {
			middleMouseDown = false;
		}
	}

	public void OnPointerDown(PointerEventData eventData){
		if (eventData.button == PointerEventData.InputButton.Middle) {
			middleMouseDown = true;
			middleMousePosition = Input.mousePosition;
			Debug.Log ("Hit position" + Input.mousePosition);
		}
	}

	public void DeleteSelection(){
		if (controlObjectSelection != null) {
			curSpace.RemoveItem (controlObjectSelection.GetMyIndex ());
		}
	}

	public void SetMyControlObjectSelection(placedObjectControl newControl){
		if (newControl == null) {

		} else {
			if (controlObjectSelection != null) {
				controlObjectSelection.GetComponent<Renderer> ().material.color = controlObjectSelection.GetMyColor ();
				controlObjectSelection.ClearInfoBlock ();
			}
			controlObjectSelection = newControl;
			controlObjectSelection.GetComponent<Renderer> ().material.color = Color.blue;
			controlObjectSelection.CreateInfoBlock ();
		}
	}

	public placedObjectControl GetObjectControl(int index){
		return objectMasterList [index];
	}

	public void OnPointerEnter(PointerEventData eventData){
		isActive = true;

	}

	public void OnPointerExit(PointerEventData eventData){
		isActive = false;

	}

	public placedObjectControl GetSelectedObjectControl(){
		return controlObjectSelection;
	}

	public placedObjectControl GetPriorityObject(Collider2D [] input){

		// placedObjectControl[] controlStorage; //Use for holding objects until they are sorted by priority

		for (int i = 0; i < input.Length; i++) {
			if (input [i].GetComponent<placedObjectControl> () != null) {
				return input [i].GetComponent<placedObjectControl> ();
			}
		}

		return null;

	}

	public placementSpace GetMySpace(){
		return curSpace;
	}

	public void UpdateMoneyDisplay(int newMoney){
		myMoneyDisplay.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Money Used : " + newMoney;
	}

	public void ClearObject(){
		GameObject.Destroy (curPrefabSelection);
		curPrefabSelection = null;
	}

	public void ClearSelection(){
		controlObjectSelection.ClearInfoBlock ();
		controlObjectSelection.GetComponent<Renderer> ().material.color = controlObjectSelection.GetComponent<placedObjectControl> ().GetMyColor ();
	}

	private bool CheckArea(){
		Collider2D[] myColliders;
		if (curPrefabSelection != null) {
			myColliders = new Collider2D[20];
			curPrefabSelection.GetComponent<Collider2D> ().OverlapCollider(new ContactFilter2D(),myColliders);
		} else {
			myColliders = Physics2D.OverlapCircleAll (myCamera.ScreenToWorldPoint (Input.mousePosition), itemClearanceArea); 
		}

		for (int i = 0; i < myColliders.Length; i++) {
			// May need to be changed ???
			if (myColliders [i] == null) {
				return true;
			}else if (myColliders [i].GetComponent<placedObjectControl> () != null ) {
				if (myColliders [i].GetComponent<placedObjectControl> ().CheckIsOverlappable () == curPrefabSelection.GetComponent<placedObjectControl> ().CheckIsOverlappable ()) {
					return false;
				}
			}
		}
			
		Debug.Log ("True");
		return true;
	}

	public int GetNewConnectId(){
		return allConnected.Count;
	}

	public void AddNewConnected(linkedObjectControl newControl){
		allConnected.Add (newControl);
	}

	public List<GameObject> GetAllConnectedObject(placedObjectAnchor inputAnchor){

		if (inputAnchor.Get


	}

	private void CheckMoveActions(){
		if (myMoveActions.Count > 0) {
			// -1 = not set, -2 = nullified, cannot be set
			int horIndx = -1;
			int vertIndx = -1;

			for (int i = 0; i < myMoveActions.Count; i++) {
				// If horz is unset, set it
				if (horIndx == -1 && (myMoveActions [i] == MoveDirection.Left || myMoveActions [i] == MoveDirection.Right)) {
					horIndx = i;
				}
				// if horz isnt -2 and the current direction is opposite to a stored horz direction, set horz to -2
				else if (horIndx != -2 && ((myMoveActions [i] == MoveDirection.Left && myMoveActions[horIndx] == MoveDirection.Right) 
					|| (myMoveActions[i] == MoveDirection.Right && myMoveActions[horIndx] == MoveDirection.Left))) {
					horIndx = -2;
				}
				// If vert is unset, set it
				else if (vertIndx == -1 && (myMoveActions [i] == MoveDirection.Up || myMoveActions [i] == MoveDirection.Down)) {
					vertIndx = i;
				}
				// if vert isnt -2 and the current direction is opposite to a stored vert direction, set vert to -2
				else if (vertIndx != -2 && ((myMoveActions [i] == MoveDirection.Up && myMoveActions[vertIndx] == MoveDirection.Down) 
					|| (myMoveActions[i] == MoveDirection.Down && myMoveActions[vertIndx] == MoveDirection.Up))) {
					vertIndx = -2;
				}
			}

			if (horIndx > -1 && vertIndx > -1) {
				Move (myMoveActions [horIndx], Mathf.Sqrt (2));
				Move (myMoveActions [vertIndx], Mathf.Sqrt (2));
			} else if (horIndx > -1) {
				Move (myMoveActions [horIndx], 1);
			} else if (vertIndx > -1) {
				Move (myMoveActions [vertIndx], 1);
			}

			myMoveActions.Clear ();

		} 

	}

	// when moving diagonally mult = root(2), otherwise = 1
	public void Move(MoveDirection myDir, float mult){
		// Get Porper direction and turn into vector
		Vector3 moveDir = Vector3.zero;
		if (myDir == MoveDirection.Up) {
			moveDir = Vector3.up;
		} else if (myDir == MoveDirection.Down) {
			moveDir = Vector3.down;
		} else if (myDir == MoveDirection.Left) {
			moveDir = Vector3.left;
		} else if (myDir == MoveDirection.Right) {
			moveDir = Vector3.right;
		} else {
			return;
		}


		Vector3 distance = moveDir * controlSingle.Instance.GetScreenMoveAmt() * mult * Time.deltaTime;

		if (curSpace.CanMoveDir (distance)) {
			myCamera.transform.Translate(distance);
			Debug.Log ("Moving" + myCamera.transform.position); 
			curSpace.SetScreenOffset (myCamera.transform.position);
		}

	}

	public void SetCamera(Vector3 newPos){
		myCamera.transform.position = newPos;
	}

	public void MoveAction(MoveDirection myDir){
		myMoveActions.Add (myDir);

	}

	public void ChangeScreenSize(bool posMovement){
		if (posMovement) {
			if (curSpace.CanIncreaseScreenSize (controlSingle.Instance.GetScreenScaleAmt ())) {
				// Enlargen
				myCamera.orthographicSize *= controlSingle.Instance.GetScreenScaleAmt ();
			}
		} else {
			// Shrink
			myCamera.orthographicSize *= 1/controlSingle.Instance.GetScreenScaleAmt ();
		}
	}

	public void CreateNoSpaceAlert(){
		Instantiate (alertNoSpacePrefab, controlSingle.Instance.GetCanvas().transform);
	}

	public void CreateNoMoneyAlert(){
		Instantiate (alertMoneyOverPrefab, controlSingle.Instance.GetCanvas().transform);
	}

	public void CreateNoItemAlert(){
		Instantiate (alertItemOverPrefab, controlSingle.Instance.GetCanvas().transform);
	}

	public void SetObject(GameObject newPrefab){
		curPrefabSelection = GameObject.Instantiate (newPrefab);
		curPrefabSelection.transform.position =  myCamera.ScreenToWorldPoint( Input.mousePosition );

	}

	public void PlayButton(){

		ClearObject ();
		curSpace.FlipPhysics ();

	}

	public void ItemButton(int mySelection){
		if (curPrefabSelection != null) {
			Destroy (curPrefabSelection);
		}

		curPrefabSelection = Instantiate( objectMasterList [mySelection].gameObject);
		curPrefabSelection.transform.position = Vector3.ProjectOnPlane( myCamera.ScreenToWorldPoint( Input.mousePosition ), Vector3.forward);

		isActive = true;

	}

	public void SectionSelectButton(int mySelection){
		if (buttonSectionSelection != -1) {
			buttonSectionList [buttonSectionSelection].SetActive (false);
		} 
		if (buttonSectionSelection == mySelection) {
			buttonSectionList [mySelection].SetActive (false);
			buttonSectionSelection = -1;
		} else {

			buttonSectionList [mySelection].SetActive (true);
			buttonSectionSelection = mySelection;
		}
	}
		

	public void LoadSpace(int saveSel){
		FileStream myFile;
		BinaryFormatter bf = new BinaryFormatter ();
		string destination = Application.persistentDataPath + controlSingle.Instance.GetCurSaveFolderName () + "\\" + "save" + controlSingle.Instance.GetLevelSeekLoc() + ".dat";

		if (File.Exists (destination)) {
			myFile = File.OpenWrite (destination);
		} else {
			myFile = File.Create (destination);
		}


		curSpace = (placementSpace) bf.Deserialize(myFile);
		List<placementObject> spaceObjects = curSpace.GetObjects ();
		controlSingle.Instance.SetScreenOffset(curSpace.GetScreenOffset());
		controlSingle.Instance.SetScreenSize (curSpace.GetScreenSize());
		controlSingle.Instance.SetScreenZoom(new Vector2( 
			Mathf.Min(controlSingle.Instance.GetDefaultZoom().x, curSpace.GetScreenSize().x),
			Mathf.Min(controlSingle.Instance.GetDefaultZoom().y, curSpace.GetScreenSize().y)));

		// Must reform to a proper resolution !!!!!!!!!!!

		myFile.Close ();

	}

	private void SaveSpace(){
		FileStream myFile;
		string destination = Application.persistentDataPath + controlSingle.Instance.GetCurSaveFolderName() + "\\" + "save" + controlSingle.Instance.GetLevelSeekLoc() + ".dat";
		BinaryFormatter bf = new BinaryFormatter ();

		if (File.Exists (destination)) {
			myFile = File.OpenWrite (destination);
		} else {
			myFile = File.Create (destination);
		}
		
		bf.Serialize (myFile, curSpace);

		myFile.Close ();
	}

	public void CreateObject(){

	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (curPrefabSelection != null) {
				curPrefabSelection.transform.position = Vector3.ProjectOnPlane( myCamera.ScreenToWorldPoint( Input.mousePosition ), Vector3.forward);
				if (!CheckArea ()) {
					curPrefabSelection.GetComponent<Renderer> ().material.color = Color.red;
					isOverlap = true;
				} else if (isOverlap) {
					curPrefabSelection.GetComponent<Renderer> ().material.color = curPrefabSelection.GetComponent<placedObjectControl> ().GetMyColor ();
					isOverlap = false;
				}
			}
		} else {
			

		}
		if (Input.GetButton ("DeleteObject")) {
			DeleteSelection ();
		}
		if (middleMouseDown) {
			Vector3 mouseChange = (middleMousePosition - Input.mousePosition);
			Debug.Log (mouseChange);
			if (mouseChange != Vector3.zero) {
				mouseChange = new Vector3 (mouseChange.x / controlSingle.Instance.GetScreenSize ().x, mouseChange.y / controlSingle.Instance.GetScreenSize ().y) * controlSingle.Instance.GetMouseScreenMoveAmt ();
				// making larger movements move more
				float xPos = Mathf.Sign (mouseChange.x);
				float yPos = Mathf.Sign (mouseChange.y);
				mouseChange = (new Vector3 (xPos * mouseChange.x, yPos * mouseChange.y) + new Vector3 (mouseChange.x * mouseChange.x, mouseChange.y * mouseChange.y));
				mouseChange = new Vector3 (Mathf.Sqrt (mouseChange.x) * xPos, Mathf.Sqrt (mouseChange.y) * yPos);
				cameraVelocity += mouseChange;
				Vector3 velocity = Vector3.zero;
				curCameraSmoothTime = Time.deltaTime;
				Vector3 newPos = Vector3.SmoothDamp (myCamera.transform.position, myCamera.transform.position + cameraVelocity, ref velocity, cameraSmoothTime - curCameraSmoothTime);
				cameraVelocity -= newPos - myCamera.transform.position;
				myCamera.transform.position = newPos; 
				middleMousePosition = Input.mousePosition;

			}
		} else if (cameraVelocity != Vector3.zero) {
			curCameraSmoothTime += Time.deltaTime;
			Vector3 velocity = Vector3.zero;

			Vector3 newPos = Vector3.SmoothDamp (myCamera.transform.position, myCamera.transform.position + cameraVelocity, ref velocity, cameraSmoothTime - curCameraSmoothTime);
			cameraVelocity -= newPos - myCamera.transform.position;
			myCamera.transform.position = newPos; 
			if (curCameraSmoothTime > cameraSmoothTime) {
				cameraVelocity = Vector3.zero; 
			}
			CheckMoveActions ();

		}else {
			CheckMoveActions ();
		}

		if (Input.GetAxis ("ScreenIncreaseSize") != 0) {
			if (Input.GetAxis("ScreenIncreaseSize") > 0) {
				ChangeScreenSize (false);
			} else {
				ChangeScreenSize (true);
			}
		}

	}

}
	