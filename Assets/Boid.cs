using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
	public Rigidbody rigid;
	private Neighborhood neighborhood;

	public Vector3 Pos {
		get { return transform.position; }
		set { transform.position = value; }
	}

	void Awake () {
		neighborhood = GetComponent<Neighborhood>();
		rigid = GetComponent<Rigidbody>();

		Pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

		Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
		rigid.velocity = vel;

		LookAhead();

		Color randColor = Color.black;
		while (randColor.r + randColor.g + randColor.b < 1.0f) {
			randColor = new Color(Random.value, Random.value, Random.value);
		}

		Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rends) {
			r.material.color = randColor;
		}

		TrailRenderer tRend = GetComponent<TrailRenderer>();
		tRend.material.SetColor("_TintColor", randColor);
	}

	void LookAhead() {
		transform.LookAt(Pos + rigid.velocity);
	}

	void FixedUpdate() {
		Vector3 vel = rigid.velocity;
		Spawner spn = Spawner.S;

		Vector3 velAvoid = Vector3.zero;
		Vector3 tooClosePos = neighborhood.avgClosePos;

		if (tooClosePos != Vector3.zero) {
			velAvoid = Pos - tooClosePos;
			velAvoid.Normalize();
			velAvoid *= spn.velocity;
		}

		Vector3 velAlign = neighborhood.avgVel;
		if (velAlign != Vector3.zero) {
			velAlign.Normalize();
			velAlign *= spn.velocity;
		}

		Vector3 velCenter = neighborhood.avgPos;
		if (velCenter != Vector3.zero) {
			velCenter -= transform.position;
			velCenter.Normalize();
			velCenter *= spn.velocity;
		}

		Vector3 delta = Attractor.POS - Pos;
		bool attracted = (delta.magnitude > spn.attractPushDist);
		Vector3 velAttract = delta.normalized * spn.velocity;

		float fdt = Time.fixedDeltaTime;
		if (velAvoid != Vector3.zero) {
			vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid * fdt);
		}
		else {
			if (velAlign != Vector3.zero) {
				vel = Vector3.Lerp(vel, velAlign, spn.velMatching * fdt);
			}
			if (velCenter != Vector3.zero) {
				vel = Vector3.Lerp(vel, velAlign, spn.flockCentering * fdt);
			}
			if (velAttract != Vector3.zero) {
				if (attracted) {
					vel = Vector3.Lerp(vel, velAttract, spn.attractPull * fdt);
				}
				else {
					vel = Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);
				}
			}
		}

		

		vel = vel.normalized * spn.velocity;

		rigid.velocity = vel;

		LookAhead();
	}
}
