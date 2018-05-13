using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour {

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
		myFullAlpha = GetComponent<UnityEngine.UI.Image> ().color.a;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive){
			if (fadeBeforeTime > 0) {
				fadeBeforeTime -= Time.deltaTime;
			} else if (fadeCur > 0) {
				fadeCur -= Time.deltaTime;
				Color newColor = GetComponent<UnityEngine.UI.Image> ().color;
				newColor.a = Mathf.Clamp01( (fadeCur/fadeAfterTime) ) * myFullAlpha;
				GetComponent<UnityEngine.UI.Image> ().color = newColor;

			} else {
				Destroy (gameObject);
			}
		}
	}

	public void MakeActive(){
		isActive = true;
	}
}
