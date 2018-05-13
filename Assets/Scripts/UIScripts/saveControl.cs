using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// TODO
// Check all Application.data path additions, make sure saveInput name has it added before it is given as a string
// Change items given to table to be only the name of the file


// Controls both the save and load objects created by the save and load in main menu
public class saveControl : menuScreen {

	int saveSel = 0;
	int curSave = controlSingle.Instance.GetSaveNum();
	string saveInputName;
	[SerializeField]
	private GameObject saveItemList;
	//[SerializeField]
	//private GameObject saveListContent;
	[SerializeField]
	private GameObject saveItemPrefab;
	[SerializeField]
	private GameObject overwriteFileAlertPrefab;
	[SerializeField][Tooltip("Determines whether this is a load or a save object.\nTrue for save, False for load")]
	private bool isSaveOb; // Save Or Load Ob
	private List<string> saveNames; // Stored in order from newest to oldest
	private List<System.DateTime> saveTimes;
	private List<string> saveInfo; // has string info on each save
	private string specialSave;
	private GameObject myCanvas;
	private string saveDir;
	[SerializeField]
	private UnityEngine.UI.InputField saveTextInput;
	[SerializeField]
	private UnityEngine.UI.Text saveTextInfo;


	void Start () {
		saveDir = Application.persistentDataPath + "\\" + controlSingle.Instance.GetSaveFolderName();
		string[] tempAry = System.IO.Directory.GetDirectories (saveDir);
		Debug.LogFormat ("Number Of Directories {0}", tempAry.Length);
		Debug.Log (saveDir);
		List<System.DateTime> tempSaveTimes = new List<System.DateTime>(tempAry.Length);
		List<string> tempNames = new List<string> (tempAry.Length);
		specialSave = controlSingle.Instance.GetCurSaveFolderName();
		saveTimes = new List<System.DateTime> ();
		saveNames = new List<string> ();

		// create the lists
		for (int i = 0; i < tempAry.Length; i++){
			tempNames.Add (tempAry [i]);
			tempSaveTimes.Add (System.IO.Directory.GetLastWriteTime (tempNames [i])); // May need file ender

		}

		// Order the lists
		for (int i = 0; i < tempNames.Count; i++) {
			int curCheck = 0;
			for (int k = 1; k < tempSaveTimes.Count; k++) {
				if (System.DateTime.Compare (tempSaveTimes[curCheck], tempSaveTimes [k]) < 0) {
					curCheck = k;
				}
			}

			saveTimes.Add (tempSaveTimes [curCheck]);
			saveNames.Add (tempNames [curCheck]);
			tempSaveTimes.RemoveAt (curCheck);
			tempNames.RemoveAt (curCheck);
			//System.IO.Directory.
		}

		CreateSaveList ();
			
	}

	void Awake(){
		myCanvas = controlSingle.Instance.GetCanvas ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Returns true if the name exists
	private bool CheckSaveName(string nameCheck){
		for (int i = 0; i < saveNames.Count; i++){
			if (string.Compare (saveNames [i], nameCheck) == 0) {
				return true;
			}
		}
		return false;
	}

	public void Save(){
		if (string.Compare (saveInputName, saveNames[curSave]) == 0) {
			if (controlSingle.Instance.GetSkipSaveDialogue ()) {
				OverWriteFile (true);
			} else {
				CreateOverwriteMenu ();
				return;
			}

		} else {
			// Create new save
			CreateNewSaveFile(saveInputName);
			//Directory.Move (Application.dataPath + "/" + controlSingle.Instance.GetSaveName (), Application.dataPath + "/" + saveName);
		}
	}

	public void NewSave(){
			// Check if the file being saved is an already existing name
		for (int i = 0; i < saveNames.Count; i++) {
			if (string.Compare (saveNames [i], saveInputName) == 0) {
				if (controlSingle.Instance.GetSkipSaveDialogue ()) {
					OverWriteFile (true);
					return;
				} else {
						//
					CreateOverwriteMenu();
					return;
				}
			}

		}
		// Write new save folder !!
		CreateNewSaveFile(saveInputName);

			
	}

	private void CreateOverwriteMenu(){
		GameObject tempOb = GameObject.Instantiate (overwriteFileAlertPrefab);
		tempOb.GetComponent<overWriteFileUI> ().SetControl (gameObject.GetComponent<saveControl> ());
		JumpForward (tempOb.GetComponent<menuScreen> ());
	}

	public void DeleteSave(){
		if (saveSel >= 0){

			Directory.Delete (saveDir + "\\" + saveNames[saveSel]);

			/*
			saveNames.RemoveAt (saveSel);
			saveTimes.RemoveAt (saveSel);
			*/

			Cleanup ();

		}
	}

	public void LoadSave(){

		foreach (string tempFiles in Directory.GetFiles (saveDir + "\\" + saveNames[saveSel])) {
			FileInfo files = new FileInfo (tempFiles);
			files.CopyTo (Path.Combine ( specialSave , files.Name), true);
		}

		//Load Game

		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


		CleanToBase ();

	}

	// Unchecked
	private void CreateNewSaveFile(string saveName){
		string folderName = saveDir + "\\" + saveName;
		Directory.CreateDirectory (folderName);
		foreach  (string tempFiles in Directory.GetFiles (specialSave)) {
			FileInfo files = new FileInfo (tempFiles);
			files.CopyTo (Path.Combine (folderName, files.Name), true);
		}

		// Create Save Info

		//   !!!!

		CleanToBase ();

		/*
		saveNames.Insert (0, folderName);
		saveTimes.Insert (0, System.DateTime.Now);
		*/
	}

	public void OverWriteFile(bool myCheck){
		if (myCheck) {
			// Copy over files
			foreach (string tempFiles in Directory.GetFiles (specialSave)) {
				FileInfo files = new FileInfo (tempFiles);
				files.CopyTo (Path.Combine (saveDir + "\\" + saveInputName, files.Name), true);
			}

			CleanToBase ();
			/*
			int posIndex = saveNames.FindIndex (Application.dataPath + saveInputName);
			saveNames.Insert (0, Application.dataPath + saveInputName);
			saveTimes.Insert (0, System.DateTime.Now);
			saveNames.RemoveAt (posIndex + 1);
			saveTimes.RemoveAt (posIndex + 1);
			*/
		} else {
			
		}
	}

	public void ChangeName(string newName){
		saveInputName = newName;
	}

	private void SaveCurSaveInfo(){
		string [] mySaveInfo = controlSingle.Instance.GetCurSaveInfo ();
		string mySaveDest = saveNames [saveSel] + "\\SaveInfo.txt";
		File.WriteAllLines (mySaveDest, mySaveInfo);
	}

	private string [] GetSaveInfo(){
		
		string destination = saveNames [saveSel] + "\\SaveInfo.txt";

		if (File.Exists (destination)) {
			return System.IO.File.ReadAllLines (destination);
		} else {
			return new string[]{"There is no information on this save", bool.FalseString};
		}
	}

	public void SaveItemClick(int index){
		if (index < 0) {
			saveSel = -1;
		} else {
			if (isSaveOb) {
				string [] saveSplit = saveNames[index].Split('\\');
				saveTextInput.text = saveSplit[saveSplit.Length -1];

			}
			saveSel = index;
			ChangeName (saveNames [index]);
			string finalString = string.Empty;
			string [] myTextAry = GetSaveInfo();
			for (int i = 0; i < myTextAry.Length; i++) {
				finalString += myTextAry [i];
			}
			saveTextInfo.text = finalString;
		}
	}

	public void SaveButtonClick(){
		if (saveSel < 0) {
			CreateNewSaveFile (saveInputName);
		} else {
			NewSave ();
		}
	}

	public void LoadButtonClick(){
		if (saveSel >= 0) {
			LoadSave ();
		}
	}
		
	private void CreateSaveList(){

		float mySpacing = 10;

		// Create list object
		float ySize = saveItemPrefab.GetComponent<RectTransform> ().sizeDelta.y;
		saveItemList.GetComponent<RectTransform>().sizeDelta = new Vector2(saveItemList.GetComponent<RectTransform>().sizeDelta.x, (ySize * (saveNames.Count + 1)) + mySpacing);
		float yPos = (saveItemList.GetComponent<RectTransform>().sizeDelta.y / 2) - (ySize / 2) - (mySpacing / 2) ;

		// Create button for new save file
		if (isSaveOb) {
			GameObject tempNewSave = GameObject.Instantiate (saveItemPrefab, saveItemList.transform);
			tempNewSave.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (tempNewSave.GetComponent<RectTransform> ().anchoredPosition.x, yPos);
			tempNewSave.GetComponent<saveListButton> ().SetMe (this, -1, "Create New Save");
			yPos -= ySize;
		}

		// Create each item and attach to list
		for (int i = 0; i < saveNames.Count; i++) {
			System.Globalization.CultureInfo myCulture = controlSingle.Instance.GetMyCulture ();
			GameObject tempItem = GameObject.Instantiate (saveItemPrefab, saveItemList.transform);
			string [] saveSplit = saveNames[i].Split('\\');
			string saveInfoText = saveSplit[saveSplit.Length - 1] + "\n" + saveTimes[i].ToString("f", myCulture);
			tempItem.GetComponent<RectTransform> ().anchoredPosition = new Vector2(tempItem.GetComponent<RectTransform> ().anchoredPosition.x, yPos);
			// Get Button script and give it the index of its save
			tempItem.GetComponent<saveListButton>().SetMe(this, i, saveInfoText);
			yPos -= ySize;
			//UnityEngine.UI.ScrollRect itemScroll; 

		}
	}


	public void CancelButton(){
		Cleanup ();
	}

	private static void ClearCurrentSave(){
		foreach (string fileStr in Directory.GetFiles(Application.persistentDataPath + "\\" + controlSingle.Instance.GetCurSaveFolderName() )) {
			FileInfo file = new FileInfo (fileStr);
			file.Delete ();
		}
	}

	public static void NewCurrentSave(){
		ClearCurrentSave ();
	}

	public override void Cleanup(){
		JumpBackMenu ();
		Destroy (gameObject);

	}

	/*
	private void CleanToBase(){
		controlSingle.Instance.ResumeGame ();
		Destroy (gameObject);
	}*/
		
}
