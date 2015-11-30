using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour {

	public GameObject objectToSpawn;
	public float horizontalOffset = 10.0f;
	public float minVerticalOffset = 0.0f;
	public float maxVerticalOffset = 3.0f;
	public float minTimeBetweenSpawns = 1.0f;
	public float maxTimeBetweenSpawns = 5.0f;

	private float spawnTimer = 0.0f;
	private float spawnTime = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		spawnTime = minTimeBetweenSpawns + Random.value*(maxTimeBetweenSpawns - minTimeBetweenSpawns);
	}
	
	// Update is called once per frame
	void Update () 
	{
		spawnTimer += Time.deltaTime;
		if(spawnTimer >= spawnTime)
		{
			Vector3 spawnPosition = Camera.main.transform.position;
			spawnPosition.x += horizontalOffset;
			spawnPosition.y = minVerticalOffset + Random.value*(maxVerticalOffset - minVerticalOffset);
			spawnPosition.z = 0;

			//Spawn object
			Instantiate(objectToSpawn, spawnPosition, objectToSpawn.transform.rotation);

			//Reset timer and time
			spawnTimer = 0.0f;
			spawnTime = minTimeBetweenSpawns + Random.value*(maxTimeBetweenSpawns - minTimeBetweenSpawns);
		}
	}
}
