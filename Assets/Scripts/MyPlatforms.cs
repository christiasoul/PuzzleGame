using UnityEngine;
using System.Collections;

public class MyPlatforms : MonoBehaviour {

	[SerializeField]
	Transform platform;
	[SerializeField]
	Transform[] locations;

	[SerializeField]
	float platformSpeed;

	int target = 0;
	int previous = 0;
	float curTime = 0;
	float maxTime;


	// !!! To make it work with array make max time calculate within SwitchDestination;

//draw and colour the path of the platform
	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube (locations[0].position,platform.localScale);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (locations[1].position,platform.localScale);
		Gizmos.color = Color.white;
		Gizmos.DrawLine (locations[0].position,locations[1].position);
	}

	void Start(){
		target += 1;
		target = target % locations.Length;
		maxTime = Vector3.Distance(locations[target].position, locations[previous].position) / platformSpeed;
	}

//set destination of platform
	void SwitchDestination(){
		target++;
		target = target % locations.Length;
		previous++;
		previous = previous % locations.Length;
		curTime = 0;
	}

//moving the platform
	void FixedUpdate(){
		curTime += Time.deltaTime;
		float timeUse = curTime / maxTime;
		platform.GetComponent<Rigidbody2D>().MovePosition(Vector3.Lerp(locations[previous].position,
				locations[target].position, timeUse * timeUse * (3f - (2f * timeUse))));
		if (curTime > maxTime){
			SwitchDestination();
		}
	}
		
}
