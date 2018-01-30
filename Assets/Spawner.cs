using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	static public Spawner S;
	static public List<Boid> boids;

	public GameObject boidprefab;
	public Transform boidAnchor;
	public int numBoids = 100;
	public float spawnRadius = 100f, spawnDelay = 0.1f;

	public float velocity = 30f, neighborDist = 30f, velMatching = 0.25f, flockCentering = 0.2f;
	public float collDist = 4f, collAvoid = 2f;
	public float attractPull = 2f, attractPush = 2f, attractPushDist = 5f;

	void Awake() {
		S = this;
		boids = new List<Boid>();
		InstantiateBoid();
	}

	public void InstantiateBoid() {
		GameObject go = Instantiate(boidprefab);
		Boid b = go.GetComponent<Boid>();
		b.transform.SetParent(boidAnchor);
		boids.Add(b);
		if(boids.Count < numBoids) {
			Invoke("InstantiateBoid", spawnDelay);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
