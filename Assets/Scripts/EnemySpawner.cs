using UnityEngine;
using System.Collections;


public class EnemySpawner : MonoBehaviour {
	public bool airSpawn;
	public bool spawning = true;
	public float spawnRate; //smaller num, more spawn
	public GameObject floorEnemy;
	public GameObject airEnemy;

	private float index;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		index += Time.deltaTime * Random.Range(.5f, 2);
		if (index > spawnRate && spawning) {
			index = 0;
			GameObject.Instantiate(airSpawn?airEnemy:floorEnemy, this.transform.position, new Quaternion());
		}
	}
}
