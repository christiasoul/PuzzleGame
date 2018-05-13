using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intGraph {
	[SerializeField]
	private static GameObject pointPrefab = null;

	private string name;

	private bool isSecFull;
	private bool hasUpperGraph;

	private int [] intData;

	private int time; // in seconds
	private int maxTime = 60;

	private float lineWidth;

	private LineRenderer myRender;
	private Vector3 graphLoc = new Vector3 (50f, 50f);
	private Color lineColor = Color.black;
	private intGraph upperGraph;

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

	public intGraph(string setName, int numOfGraphs, int[] inTime, Color [] setColors) {
		name = setName;
		lineWidth = controlSingle.Instance.getGraphWidth ();

		intData = new int[maxTime];

		maxTime = inTime [numOfGraphs - 1];
		lineColor = setColors [numOfGraphs - 1];

		if (numOfGraphs > 1) {
			hasUpperGraph = true;
			upperGraph = new intGraph (setName,  --numOfGraphs, inTime, setColors);
		} else {
			hasUpperGraph = false;
		}

	}


	// Used to load
	public intGraph(string setName, int numOfGraphs, int[] inTime, Color [] setColors, List<int []> loadData, List<int> timePos) {
		name = setName;
		lineWidth = controlSingle.Instance.getGraphWidth ();

		intData = new int[maxTime];

		maxTime = inTime [numOfGraphs - 1];
		lineColor = setColors [numOfGraphs - 1];

		if (numOfGraphs > 1) {
			hasUpperGraph = true;
			upperGraph = new intGraph (setName, --numOfGraphs, inTime, setColors);
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
				tempPoints.Add (new Vector3 (((float)counter++) / xAxisMax, ((float)intData [index]) / yAxisMax));
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

	public void InputData(int value){
		intData [time] = value;
		if (hasUpperGraph == true && time++ == maxTime) {
			time = 0;
			isSecFull = true;
			upperGraph.InputData (value);

		}
	}

	private intGraph RetUpper(int numUp){
		if (numUp <= 1 && upperGraph != null) {
			return upperGraph;
		} else {
			return RetUpper (--numUp);
		}
	}

	public float GetAverage(int numUp){
		if (numUp <= 0) {
			int sum = 0;
			for (int i = 0; i < intData.Length; i++) {
				sum += intData [i];
			}
			return ((float)sum) / ((float)intData.Length);
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

	public int GetMax(int numUp){
		if (numUp <= 0) {
			int maxNum = 0;
			for (int i = 0; i < intData.Length; i++) {
				maxNum = Mathf.Max (maxNum, intData [i]);
			}
			return maxNum;
		} else {
			return upperGraph.GetMax (--numUp);
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
		if (upNum > 0 && upperGraph != null) {
			return upperGraph.GetUpperLoadData (--upNum);
		} else {
			return new graphData (
				time, intData, isSecFull, name, lineColor
			);
		}

	}
	/*
	public static void SetPointPrefab(GameObject newPrefab){
		pointPrefab = newPrefab;
	}*/
}

public struct graphData{
	public int timePos;
	public bool isFull;
	public float[] floatData;
	public int[] intData;
	public string myName;
	public Color myColor;

	public graphData(int inTimePos, float [] inFloatData, bool inFull, string inName, Color inColor ){
		timePos = inTimePos;
		isFull = inFull;
		floatData = inFloatData;
		intData = null;
		myName = inName;
		myColor = inColor;


	}

	public graphData(int inTimePos, int [] inIntData, bool inFull, string inName, Color inColor ){
		timePos = inTimePos;
		isFull = inFull;
		floatData = null;
		intData = inIntData;
		myName = inName;
		myColor = inColor;


	}

}