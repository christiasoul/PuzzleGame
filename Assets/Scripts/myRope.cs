using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myRope : MonoBehaviour {

	[SerializeField]
	private Transform ropeConnectStart;
	[SerializeField]
	private Transform ropeConnectEnd;
	[SerializeField]
	private int numOfSections;
	[SerializeField]
	private float sectionLength;
	[SerializeField]
	[Tooltip("Spring Constant")]
	private float kRope = 40f;
	[SerializeField]
	[Tooltip("Rope Friction")]
	private float dRope = 2;
	[SerializeField]
	[Tooltip("Air Resistance")]
	private float aRope = 0.05f;
	[SerializeField]
	[Tooltip("Rope Section Mass")]
	private float mRopeSection = .2f;
	[SerializeField]
	[Tooltip("Number of times the simulation runs")]
	private int accuracy = 1;
	[SerializeField]
	private float ropeWidth = 0.2f;
	[SerializeField]
	private float stretchRange = .1f;

	private bool startIsAnchored;
	private bool endIsAnchored;

	private int anchorNum = 0;


	private List<ropeSec> myRopeSections = new List<ropeSec> ();
	[SerializeField]
	private LineRenderer lineRender;
	// Use this for initialization
	void Awake () {
		lineRender = gameObject.GetComponent<LineRenderer> ();

		Vector3 pos = ropeConnectEnd.position;

		List<Vector3> ropePositions = new List<Vector3> ();


		for (int i = 0; i < numOfSections; i++) {

			ropePositions.Add (pos);

			pos.y -= sectionLength;

		}

		for (int i = ropePositions.Count - 1 ; i >= 0; i--) {

			myRopeSections.Add(new ropeSec(ropePositions[i]));
		}

		lineRender.startWidth = ropeWidth;
		lineRender.endWidth = ropeWidth;

	}
	
	// Update is called once per frame
	void Update () {

		Display ();

		DebugLength ();

		// Move any loose ends to the rope position end

	}

	void FixedUpdate(){

		float timeStep = Time.fixedDeltaTime / (float)accuracy;

		for (int i = 0; i < accuracy; i++) {
			UpdateRopeSimulation (myRopeSections, timeStep);
		}

	}

	private void Display(){

		lineRender.startWidth = ropeWidth;
		lineRender.endWidth = ropeWidth;

		Vector3[] positions = new Vector3[myRopeSections.Count];

		for (int i = 0; i < myRopeSections.Count; i++) {
			positions [i] = myRopeSections [i].pos;
		}

		lineRender.positionCount = positions.Length;

		lineRender.SetPositions (positions);



	}

	private void DebugLength(){

		Vector3[] positions = new Vector3[myRopeSections.Count];

		for (int i = 0; i < myRopeSections.Count; i++) {
			positions [i] = myRopeSections [i].pos;
		}

		lineRender.positionCount = positions.Length;

		lineRender.SetPositions (positions);

	}

	private void UpdateRopeSimulation(List<ropeSec> setRopeSections, float timeStep){

		// Setting the last rope section to the end position (since it doesnt move)
		ropeSec lastRopeSec = setRopeSections[setRopeSections.Count - 1];
		lastRopeSec.pos = ropeConnectStart.position;
		setRopeSections [setRopeSections.Count - 1] = lastRopeSec;

		List<Vector3> accelerations = CalculateAccelerations (setRopeSections);

		List<ropeSec> nextPosVelForwardEuler = new List<ropeSec> ();

		// Should use count -1 or not?
		for (int i = 0; i < setRopeSections.Count - 1; i++) {
			ropeSec thisRopeSec = ropeSec.zero;

			// Velocity
			thisRopeSec.vel = setRopeSections[i].vel + (accelerations[i] * timeStep);
			thisRopeSec.pos = setRopeSections [i].pos + (setRopeSections [i].vel * timeStep);
			nextPosVelForwardEuler.Add (thisRopeSec);

		}

		// add last for pos vec

		List<Vector3> accelFromEul = CalculateAccelerations (nextPosVelForwardEuler);
		List<ropeSec> nextPosVelHeunsMethod = new List<ropeSec> ();

		for (int i = 0; i < setRopeSections.Count - 1; i++) {
			ropeSec thisRopeSection = ropeSec.zero;

			thisRopeSection.vel = setRopeSections [i].vel + ((accelerations [i] + accelFromEul [i]) * .5f * timeStep);
			thisRopeSection.pos = setRopeSections [i].pos + ((setRopeSections [i].vel + nextPosVelForwardEuler [i].vel) * .5f * timeStep);
			nextPosVelHeunsMethod.Add (thisRopeSection);

		}

		// Add last if not already added

		for (int i = 0; i < setRopeSections.Count - 1; i++) {
			setRopeSections [i] = nextPosVelHeunsMethod [i];

			//setRopeSEctions[i] = nextPosVelForwardEuler[i];

		}

		int maximumStretchIterations = 2;

		for (int i = 0; i < maximumStretchIterations; i++) {
			MaxStretch (setRopeSections);

		}



	}

	private List<Vector3> CalculateAccelerations(List<ropeSec> setRopeSections){

		List<Vector3> accelerations = new List<Vector3> ();

		float kr = kRope;
		float dr = dRope;
		float ar = aRope;
		float m = mRopeSection;
		float wantedLength = sectionLength;


		List<Vector3> allForces = new List<Vector3> ();

		for (int i = 0; i < setRopeSections.Count - 1; i++) {

			Vector3 vectorBetween = setRopeSections [i + 1].pos - setRopeSections [i].pos;

			float distanceBetween = vectorBetween.magnitude;

			Vector3 dir = vectorBetween.normalized;

			float springForce = kr * (distanceBetween - wantedLength);

			// Damping
			float frictionForce = dr * ((Vector3.Dot(setRopeSections[i + 1].vel - setRopeSections[i].vel, vectorBetween)) / distanceBetween );

			Vector3 springForceVec = -1 * (springForce + frictionForce) * dir;

			springForceVec = springForceVec * -1;

			allForces.Add (springForceVec);

		}

		for (int i = 0; i < setRopeSections.Count - 1; i++) {

			Vector3 springForce = Vector3.zero;

			springForce += allForces [i];


			if (i != 0) {
				springForce -= allForces [i - 1];
			}

			float vel = setRopeSections [i].vel.magnitude;

			Vector3 dampingForce = ar * vel * vel * setRopeSections [i].vel.normalized;

			float springMass = m;

			if (i == 0) {
				// ignores start
				springMass += ropeConnectStart.GetComponent<Rigidbody>().mass;
			}

			Vector3 gravityForce = springMass * new Vector3 (0f, -9.81f, 0f);

			Vector3 totalForce = springForce + gravityForce - dampingForce;

			Vector3 acceleration = totalForce / springMass;

			accelerations.Add (acceleration);

		}
		// Used to end last acceleration
		accelerations.Add (Vector3.zero);

		return accelerations;
	}

	private void MaxStretch(List<ropeSec> setRopeSections){

		float maxStretch = 1.0f + stretchRange;
		float minStretch = 1.0f - stretchRange;

		for (int i = setRopeSections.Count - 1; i > 0; i--){

			ropeSec topSection = setRopeSections [i];
			ropeSec bottomSection = setRopeSections [i - 1];

			float dist = (topSection.pos - bottomSection.pos).magnitude;

			float stretch = dist / sectionLength;

			if (stretch > maxStretch) {

				float compressLength = dist - (sectionLength * maxStretch);

				Vector3 compressDir = (topSection.pos - bottomSection.pos).normalized;
				compressDir *= compressLength;
				MoveSection (compressDir, i - 1);


			} else if (stretch < minStretch) {

				float stretchLength = (sectionLength * minStretch) - dist;

				Vector3 stretchDir = (topSection.pos - bottomSection.pos).normalized;
				stretchDir *= stretchLength;
				MoveSection (stretchDir, i - 1);
			}

		}


	}

	private void MoveSection(Vector3 finalChange, int listPos){

		ropeSec bottomSection = myRopeSections [listPos];

		Vector3 pos = bottomSection.pos;
		pos += finalChange;
		bottomSection.pos = pos;

		myRopeSections [listPos] = bottomSection;

	}

	public int attach(){

		return anchorNum++;

	}

	public void setAnchor(int position, bool isAnchored){
		if (position == 0) {
			startIsAnchored = isAnchored;
		} else if (position == 1) {
			endIsAnchored = isAnchored;

		}

	}



}
