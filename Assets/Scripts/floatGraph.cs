using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatGraph {
	[SerializeField]
	private static GameObject pointPrefab = null;

	private string name;

	private bool isSecFull;
	private bool hasUpperGraph;

	private float [] floatData;

	private int time; // in seconds
	private int maxTime = 60;

	private float lineWidth;

	private LineRenderer myRender;
	private Vector3 graphLoc = new Vector3 (50f, 50f);
	private Color lineColor = Color.black;
	private floatGraph upperGraph;

	public string getName(){
		return name;
	}

	/*
	public graph(string setName, bool setFloat) {
		name = setName;
		isFloat = setFloat;
		lineWidth = controlSingle.Instance.getGraphWidth ();

		if (isFloat) {
			floatData = new float[maxTime];
		} else {
			intData = new int[maxTime];
		}
		hasUpperGraph = false;
	}
	*/

	public floatGraph(string setName, int numOfGraphs, int[] inTime, Color [] setColors) {
		name = setName;
		lineWidth = controlSingle.Instance.getGraphWidth ();

		floatData = new float[maxTime];

		maxTime = inTime [numOfGraphs - 1];
		lineColor = setColors [numOfGraphs - 1];

		if (numOfGraphs > 1) {
			hasUpperGraph = true;
			upperGraph = new floatGraph (setName, --numOfGraphs, inTime, setColors);
		} else {
			hasUpperGraph = false;
		}

	}


	// Used to load
	public floatGraph(string setName, int numOfGraphs, int[] inTime, Color [] setColors, List<float []> loadData, List<int> timePos) {
		name = setName;
		lineWidth = controlSingle.Instance.getGraphWidth ();

		floatData = new float[maxTime];


		maxTime = inTime [numOfGraphs - 1];
		lineColor = setColors [numOfGraphs - 1];


		if (numOfGraphs > 1) {
			hasUpperGraph = true;
			upperGraph = new floatGraph (setName, --numOfGraphs, inTime, setColors);
		} else {
			hasUpperGraph = false;
		}

	}


	public void Display(float xAxisMax, float yAxisMax, int upNum){
		if (upNum > 0) {
			upperGraph.Display (xAxisMax, yAxisMax, --upNum);
		} else {
			if (myRender == null) {
				CreateLineRenderer ();
			}

			List<Vector3> tempPoints = new List<Vector3> ();
			int index;
			int length = Mathf.Min ((int)xAxisMax, maxTime);
			int end;
			int counter = 0;
			if (isSecFull) {
				index = (time + (maxTime - length)) % maxTime;
				end = time - 1;
			} else {
				index = 0;
				end = time;
			}
				
			for (; index != end; index = (index + 1) % maxTime) {
				tempPoints.Add (new Vector3 (((float)counter++) / xAxisMax, floatData [index] / yAxisMax));
			}

			myRender.SetPositions (tempPoints.ToArray ());
		}

	}

	private void CreateLineRenderer(){
		GameObject lineOb = new GameObject ();
		lineOb.transform.position = graphLoc;
		//GameObject line = GameObject.Instantiate (controlSingle.Instance.getGraphLocation());

		myRender = lineOb.AddComponent<LineRenderer> ();
		myRender.endColor = lineColor;
		myRender.startColor = lineColor;
		myRender.startWidth = lineWidth;
		myRender.endWidth = lineWidth;

	}


	public void ClearDisplay(){

		GameObject.Destroy (myRender.gameObject);
	}


	public void SetColor(Color newColor){
		Color lineColor = newColor;
		myRender.startColor = lineColor;
		myRender.endColor = lineColor;
	}

	public void InputData(float value){
		floatData [time] = value;
		if (hasUpperGraph == true && time++ == maxTime) {
			time = 0;
			isSecFull = true;
			upperGraph.InputData (value);

		}
	}
		

	private floatGraph RetUpper(int numUp){
		if (numUp <= 1 && upperGraph != null) {
			return upperGraph;
		} else {
			return upperGraph.RetUpper (--numUp);
		}
	}

	public float GetAverage(int numUp){
		if (numUp <= 0 && upperGraph != null) {
			float sum = 0;
			for (int i = 0; i < floatData.Length; i++) {
				sum += floatData [i];
			}
			return ((float)sum) / ((float)floatData.Length);
		} else {
			/*
			if (upperGraph) {
				return upperGraph.GetAverage (--numUp);
			} else {
				Debug.LogError ("Can't get average of floatGraph");
			}*/

			return upperGraph.GetAverage (--numUp);
		}
	}

	public float GetMax(int numUp){
		if (numUp <= 0 && upperGraph != null) {
			float maxNum = 0;
			for (int i = 0; i < floatData.Length; i++) {
				maxNum = Mathf.Max (maxNum, floatData [i]);
			}
			return maxNum;
		} else {
			return  upperGraph.GetMax (--numUp);
		}
	}

	private int GetUpperDepth(){
		if (upperGraph != null) {
			return upperGraph.GetUpperDepth () + 1;
		} else {
			return 0;
		}
	}

	// Remove and just have public getters??
	public List<graphData> GiveLoadData(){
		List<graphData> retGraph = new List<graphData>();
		int length = GetUpperDepth ();
		for (; length > 0; length--) {
			retGraph.Add(GetUpperLoadData(length));
		}
		return retGraph;
	}

	public graphData GetUpperLoadData(int upNum){
		if (upNum > 0) {
			return upperGraph.GetUpperLoadData (--upNum);
		} else {
			return new graphData (
				time, floatData, isSecFull, name, lineColor
			);
		}

	}

	/*
	//Only singleton uses
	public static void SetPointPrefab(GameObject newPrefab){
		pointPrefab = newPrefab;
	}*/

}
/*
public struct graphData{
	public int timePos;
	public bool isFloat;
	public float[] floatData;
	public int[] intData;

	public graphData(int inTimePos, bool inIsFloat, float [] inFloatData){



	}

}
*/