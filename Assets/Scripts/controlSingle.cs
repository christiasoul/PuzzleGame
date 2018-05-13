using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class controlSingle {

	private static controlSingle instance;
	private readonly string[] otherPrefabNames = { // Name all prefabs
		//"pointPrefab",
		//"toolBoxPrefab",
		"mainMenuPrefab",

		//

	};

	private readonly string[] physicsPrefabNames = { // Name all prefabs
		//"pointPrefab",
		//"toolBoxPrefab",
		"spherePrefab.prefab",
		"cubePrefab.prefab"

		//

	};

	private readonly string[] levelNames = {
		"Energy",
		"Battery"

	};

	private readonly string saveFolderName = "save"; // Put the whole curSave folder into a new folder here and name it what the player wants
	private readonly string curSaveFolderName = "current"; // Put all data that is in the current save here
	private readonly string metaSaveName = "options"; // Store options and things that are carried over from game to game
	private readonly string schematicsFolderName = "schematics"; // stores setup for levels that can be loaded

	private const string playModeCanvasName = "playCanvas";
	private const string titleMenuCanvasName = "menuCanvas";

	private controlSingle(){
		//intGraph.SetPointPrefab(

		/*
		string [] tempAry = Directory.GetDirectories (Application.dataPath);
		for (int i = 0; i < tempAry.Length; i++) {
			saveNames.Add (tempAry [i]);
		}*/
		mainMenuPrefab = Resources.Load ("prefabs/" + otherPrefabNames [0], typeof( GameObject )) as GameObject;
		if (mainMenuPrefab == null) { 
			Debug.LogError (otherPrefabNames [0] + " is missing");
		}

		if (GameObject.Find (titleMenuCanvasName) != null) {
			myCanvas = GameObject.Find (titleMenuCanvasName);
		} else if (GameObject.Find (playModeCanvasName) != null) {
			myCanvas = GameObject.Find (playModeCanvasName);
			placementControl.instance.StartFreePlay (new Vector3 (200f, 200f));
		}

		screenWidth = Screen.width;
		screenHeight = Screen.height;
		ScreenSize = new Vector2 (1920, 1080);

	}

	public static controlSingle Instance{
		get{
			if(instance == null){
				instance = new controlSingle ();
			}
			return instance;
		}
	}

	private Vector3 graphLocation;
	private float graphWidth = .2f; //width of lines
	public Vector3 getGraphLocation(){
		return graphLocation;
	}
	public float getGraphWidth(){
		return graphWidth;
	}

	private Vector2 ScreenSize;
	private Vector2 ScreenOffset;
	private Vector2 ScreenZoom;
	private Vector2 DefaultZoom;

	public Vector2 GetScreenSize(){
		return ScreenSize;
	}
	public Vector2 GetScreenOffset(){
		return ScreenOffset;
	}
	public Vector2 GetScreenZoom(){
		return ScreenZoom;
	}
	public void SetScreenZoom(Vector2 newScreenZoom){
		ScreenZoom = newScreenZoom;
	}
	public void SetScreenOffset(Vector2 newScreenOffset){
		ScreenOffset = newScreenOffset;
	}
	public void SetScreenSize(Vector2 newScreenSize){
		ScreenSize = newScreenSize;
	}
	public Vector2 GetDefaultZoom(){
		return DefaultZoom;
	}


	private GameObject pointPrefab;
	private GameObject toolBoxPrefab;
	private GameObject myCanvas;

	private GameObject mainMenuPrefab;
	private menuScreen curMenu;



	private physicsGameControl gamePhysicsControl;

	/*
	private List<GameObject> controlObjectPrefabs;
	private List<GameObject> physicsObjectPrefabs;

	public GameObject GetControlPrefab(int myIndex){
		return controlObjectPrefabs [myIndex];
	}

	public GameObject GetPhysicsPrefab(int myIndex){
		return physicsObjectPrefabs [myIndex];
	}

	public placementObject CreatePlacementObject(Vector3 pos, int setIndex, int setId){
		return new placementObject (setIndex, pos, controlObjectPrefabs [setId],
			physicsObjectPrefabs [setId]);
	}*/

	public physicsGameControl GetPhysicsControl(){
		return gamePhysicsControl;
	}

	public void SetPhysicsControl(physicsGameControl newControl){
		gamePhysicsControl = newControl;
	}

	public menuScreen GetCurMenu(){
		return curMenu;
	}



	private float screenMoveAmt = 20f;
	private float mouseScreenMoveAmt = 60f;
	private float screenScaleAmt = 1.2f;

	public float GetScreenMoveAmt(){
		return screenMoveAmt;
	}

	public float GetScreenScaleAmt(){
		return screenScaleAmt;
	}

	public float GetMouseScreenMoveAmt(){
		return screenMoveAmt;
	}
	//private void getPrefabs

	private int saveNum;
	private const int numOfExtraSeekLoc = 1;
	private int levelNum;
	//private string specialSaveName;

	// Save Info 
	private int lastPlayedLevel;
	private float totalTimePlayed;
	private float lastTimeCheckedTime = 0;
	// **********

	public int GetSaveNum(){
		return saveNum;
	}

	public void SetSaveNum(int newSave){
		saveNum = newSave;
	}

	public void SetLevelNum(int newNum){
		levelNum = newNum;
		lastPlayedLevel = newNum;
	}

	public int GetLevelNum(){
		return levelNum;
	}

	public int GetLevelSeekLoc(){
		return levelNum + numOfExtraSeekLoc;
	}

	public string GetLevelName(int inNum){
		if (inNum > levelNames.Length || inNum < 0) {
			return "Level Not Found";
		} else {
			return levelNames [inNum];
		}
	}

	public float GetTotalTimePlayed(){
		totalTimePlayed += Time.realtimeSinceStartup - lastTimeCheckedTime;
		lastTimeCheckedTime = Time.realtimeSinceStartup;
		return totalTimePlayed;
	}

	public void NewGameSetup(){
		lastPlayedLevel = 0;
		totalTimePlayed = 0;

	}

	public void FirstTimeSetup(){
		if (Directory.Exists (Application.persistentDataPath + "\\" + curSaveFolderName)) {

		} else {
			Directory.CreateDirectory (Application.persistentDataPath + "\\" + curSaveFolderName);
		}

		if (Directory.Exists (Application.persistentDataPath + "\\" + saveFolderName)) {

		} else {
			Directory.CreateDirectory (Application.persistentDataPath + "\\" + saveFolderName);
		}

		if (Directory.Exists (Application.persistentDataPath + "\\" + metaSaveName)) {

		} else {
			Directory.CreateDirectory (Application.persistentDataPath + "\\" + metaSaveName);
			CreateDefaultOptions ();
		}

		if (Directory.Exists (Application.persistentDataPath + "\\" + schematicsFolderName)) {

		} else {
			Directory.CreateDirectory (Application.persistentDataPath + "\\" + schematicsFolderName);
		}
			
	}
		
	/*
	public bool CheckSaveName(string nameCheck){
		for (int i = 0; i < saveNames.Count; i++) {
			if (string.Compare(nameCheck, saveNames[i])){
				return true;
			}
		}
		return false;
	}*/

	public System.Globalization.CultureInfo GetMyCulture(){
		return myCulture;
	}

	/*
	public string GetSpecialSaveName(){
		return specialSaveName;
	}*/

	// If the special name needs to be reset and changed to another andom name
	/*
	public string ResetSpecialSaveName(){
		string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		string stringChars = string.Empty;
		const int stringLength = 16;
		for (int i = 0; i < stringLength; i++)
		{
			stringChars += chars[Random.Range(0, stringLength)];
		}

		specialSaveName = stringChars;
		return stringChars;
	}
	*/

	public string GetSaveFolderName(){
		return saveFolderName;
	}

	public string GetCurSaveFolderName(){
		return curSaveFolderName;
	}

	// Saves a file that holds general info for this current save such as current date or last played level.
	public void SaveShort(){


	}

	public void LoadShort(){

	}

	public GameObject GetCanvas(){
		return myCanvas;
	}

	public void SetCanvas(GameObject newCanvas){
		myCanvas = newCanvas;
	}

	public void FreePlay(GameObject newCanvas){
		myCanvas.SetActive (false);
		myCanvas = newCanvas;
		myCanvas.SetActive (true);
		placementControl.instance.StartFreePlay (new Vector3 (4000f, 4000f));
	}

	// Main Menu

	public void CreateMenuPopup(){
		if (IsMenu() == false) {
			curMenu = GameObject.Instantiate (mainMenuPrefab, myCanvas.transform).GetComponent<menuScreen>();
			PauseGame ();
		} else {
			Debug.LogError ("The game is trying to create the main menu but there is already a menu item");
		}
	}

	public void RemoveMenu(){
		curMenu.Cleanup ();
	}

	public bool IsMenu(){
		if (curMenu != null) {
			return true;
		} else {
			return false;
		}
	}

	public void SetMenu(menuScreen newMenu){
		curMenu = newMenu;
	}

	public void PauseGame(){

	}

	public void ResumeGame(){

	}

	public string [] GetCurSaveInfo(){
		return new string []{
			true.ToString(),
			totalTimePlayed .ToString(),
			skipSaveDialogue.ToString(),
			hintsOn.ToString(),
			cursorCustomBorderSize.ToString()
		};
	}

	private void SetCurSaveInfo(string [] myString){
		totalTimePlayed = System.Single.Parse (myString [1]);
		skipSaveDialogue = System.Boolean.Parse (myString [2]);
		hintsOn = System.Boolean.Parse (myString [3]);
		cursorCustomBorderSize = System.Int32.Parse (myString [4]);

	}

	// Options

	private System.Globalization.CultureInfo myCulture;
	private bool skipSaveDialogue;
	private int screenWidth;
	private int screenHeight;
	private bool hintsOn;
	private Color cursorCustomColor;
	private Color cursorCustomBorderColor;
	private int cursorCustomBorderSize;

	// As per area
	private bool freePlay = true; // Items have no cost, and cost is not displayed;

	public bool IsFreePlay(){
		return freePlay;
	}

	public void SetFreePlay(bool decision){
		freePlay = decision;
	}


	private void CreateDefaultOptions(){
		skipSaveDialogue = false;

		hintsOn = true;
		cursorCustomBorderColor = Color.black;
		cursorCustomBorderSize = 2;
	}

	private void SaveOptions(){
		string [] mySaveInfo = new string[]{
			skipSaveDialogue.ToString(),
			hintsOn.ToString(),
			cursorCustomBorderColor.ToString(),
			cursorCustomBorderSize.ToString()
		};

		string fileString = string.Empty;
		bool myContinue = true;
		for (int i = 0;myContinue;) {
			fileString += mySaveInfo [i];
			if (++i < mySaveInfo.Length) {
				fileString += "\n";
			} else {
				myContinue = false;
			}
		}


	}

	public bool GetSkipSaveDialogue(){
		return skipSaveDialogue;
	}

	public void CheckSkipSaveDialogue(){
		skipSaveDialogue = true;
	}
		

	public void LoadOptions(){
		if (Directory.Exists (Application.persistentDataPath + "\\" + metaSaveName)) {
			LoadGlobalOptions ();
		} else {
			FirstTimeSetup ();
		}

	}
	// A Save file that does options between seperate saves
	private void LoadGlobalOptions(){
		//skipSaveDialogue


	}
}
