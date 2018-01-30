using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {
	static public Vector3 POS = Vector3.zero;

	public float radius = 10, xPhase = 0.5f, yPhase = 0.4f, zPhase = 0.1f;
	
	void FixedUpdate() {
		Vector3 scale = transform.localScale;
		Vector3 tPos = new Vector3( 
			Mathf.Sin(xPhase * Time.time) * radius * scale.x,
			Mathf.Sin(yPhase * Time.time) * radius * scale.y,
			Mathf.Sin(zPhase * Time.time) * radius * scale.z
		);
		transform.position = tPos;
		POS = tPos;
	}
}