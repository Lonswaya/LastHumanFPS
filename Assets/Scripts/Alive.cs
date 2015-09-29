using UnityEngine;
using System.Collections;

public class Alive : MonoBehaviour {
	public float time = 60;
	private float timeSince;
	public bool hole;
	// Use this for initialization
	void Start () {
		if (hole) {
			this.GetComponent<AudioSource>().priority = Random.Range(0, 30);
			this.GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeSince+= Time.deltaTime;
		if (timeSince > time) GameObject.Destroy(this.gameObject);
		if (hole && this.GetComponent<AudioSource>()) {
				if (this.GetComponent<AudioSource>().clip.length > timeSince) Component.Destroy(this.GetComponent<AudioSource>());
		}
	}
}
