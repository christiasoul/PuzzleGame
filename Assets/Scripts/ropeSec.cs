using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ropeSec  {
	public Vector3 pos;
	public Vector3 vel;

	public static readonly ropeSec zero = new ropeSec(Vector3.zero);

	public ropeSec(Vector3 pos){
		this.pos = pos;

		this.vel = Vector3.zero;

	}
}
