using UnityEngine;
using System.Collections;

public class Alive : MonoBehaviour {
	public float time = 60;
	private float timeSince;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeSince+= Time.deltaTime;
		if (timeSince > time) GameObject.Destroy(this.gameObject);
	}
}
