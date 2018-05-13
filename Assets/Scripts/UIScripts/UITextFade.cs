using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextFade : MonoBehaviour {

	private bool isActive;
	[SerializeField]
	[Tooltip("The time before the fade starts")]
	private float fadeBeforeTime;
	[SerializeField]
	[Tooltip("The time until the fade ends after it has started")]
	private float fadeAfterTime;
	private float fadeCur;
	private float myFullAlpha;

	// Use this for initialization
	void Start () {
		MakeActive ();
		fadeCur = fadeAfterTime;
		myFullAlpha = GetComponent<UnityEngine.UI.Text> ().color.a;
	}

	// Update is called once per frame
	void Update () {
		if (isActive){
			if (fadeBeforeTime > 0) {
				fadeBeforeTime -= Time.deltaTime;
			} else if (fadeCur > 0) {
				fadeCur -= Time.deltaTime;
				Color newColor = GetComponent<UnityEngine.UI.Text> ().color;
				newColor.a = Mathf.Clamp01( (fadeCur/fadeAfterTime) ) * myFullAlpha;
				GetComponent<UnityEngine.UI.Text> ().color = newColor;

			} 
		}
	}

	public void MakeActive(){
		isActive = true;
	}

}
