using UnityEngine;
using System.Collections;


public class EnemySpawner : MonoBehaviour {
	public bool airSpawn;
	public bool spawning = true; //is spawning in the map
	public float spawnRate; //seconds in between spawning
	public GameObject floorEnemy;
	public GameObject airEnemy;

	private bool pause; //pausing for being too large
	private float spawnTime;
	private float index;
	private int lastSpawnTime;
	// Use this for initialization
	void Start () {
		index = spawnRate * .9f;
	}
	
	// Update is called once per frame
	void Update () {
		spawnRate -= Time.deltaTime * .002f * spawnRate;
		index += Time.deltaTime ;
		//print(index + " " + spawnRate);
		if (((int)index) % (int)spawnRate == 0 && spawning && !pause && (int)index != lastSpawnTime) {
			lastSpawnTime = (int)index;
			GameObject.Instantiate(airSpawn?airEnemy:floorEnemy, this.transform.position, new Quaternion());
		}

		if (index % 10 == 0) {
			pause =  (GameObject.FindObjectsOfType<Enemy>().Length > 40);
		}
	}
}
